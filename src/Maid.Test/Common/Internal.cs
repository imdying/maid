using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Maid.Test;

internal static class Internal
{
    private static Stopwatch? _stopwatch;

    [ModuleInitializer]
    public static void Benchmark()
    {
        _stopwatch = Stopwatch.StartNew();
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
    }

    public static int InvokeProcess(string args, bool debug = false)
    {
        if (string.IsNullOrWhiteSpace(args))
            throw new ArgumentNullException(nameof(args));

        using var process = new Process();

        process.StartInfo = new()
        {
            FileName = "dotnet",
            Arguments = $"{Assembly.GetExecutingAssembly().Location} {args}",
            UseShellExecute = debug,
            CreateNoWindow = !debug
        };

        process.Start();
        process.WaitForExit();

        return process.ExitCode;
    }

    private static void OnProcessExit(object? sender, EventArgs e)
    {
        Console.WriteLine("\nTime elapsed: {0} ms", _stopwatch?.ElapsedMilliseconds);
    }
}
