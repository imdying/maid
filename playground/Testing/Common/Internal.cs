using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Testing;

internal static class Internal
{
    private static Stopwatch? _stopwatch;

    [ModuleInitializer]
    public static void Benchmark()
    {
        _stopwatch = Stopwatch.StartNew();
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
    }

    private static void OnProcessExit(object? sender, EventArgs e)
    {
        Console.WriteLine("\nTime elapsed: {0} ms", _stopwatch?.ElapsedMilliseconds);
    }
}
