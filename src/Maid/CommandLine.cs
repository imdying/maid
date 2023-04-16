using System.CommandLine;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Maid.Parsing;
using Maid.Properties;

namespace Maid;

/// <summary>
/// Provides a command-line interpreter for command-line interface applications.
/// </summary>
public sealed class CommandLine : ICommandLineFluentInterface<CommandLine>
{
    private static bool _verboseEnabled = true;
    private readonly CliConfiguration _cfg;
    private readonly CliRootCommand _root;

    public CommandLine()
    {
        _root = new(Resources.SymbolNoDescription);
        _cfg = new(_root);
        Parser = new(this);
    }

    /// <summary>
    /// Gets or sets the <see cref="System.Reflection.Assembly">assembly</see> which contains the <see cref="Command">commands</see>.
    /// </summary>
    internal Assembly Assembly { get; private set; } = Assembly.GetCallingAssembly();

    internal CommandLineParser Parser
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets or sets the prefix used for options.
    /// </summary>
    /// <remarks>
    /// In Unix/Linux systems and its derivatives, such as macOS and Ubuntu, the double hyphen (--option) is commonly used as a prefix to indicate a long option, typically more descriptive and preceded by a word or words.
    /// <br/>
    /// <br/>
    /// In Windows operating systems, the forward slash (/option) is commonly used as a prefix to indicate a short option, typically one or two characters.
    /// </remarks>
    internal string? Prefix { get; private set; } = "/";
    /// <summary>
    /// Kills the application and exit with a non-zero exit code.
    /// </summary>
    [DoesNotReturn]
    public static void Interrupt() => Terminate(-1);

    /// <summary>
    /// Sets the verbosity of the logging methods.
    /// </summary>
    /// <param name="verboseEnabled">Whether or not verbose output should be enabled.</param>
    public static void SetVerbose(bool verboseEnabled) => _verboseEnabled = verboseEnabled;

    /// <summary>
    /// Kills the application.
    /// </summary>
    /// <param name="code">The exit code value to return.</param>
    [DoesNotReturn]
    public static void Terminate(int code)
#pragma warning disable CS8763 // A method marked [DoesNotReturn] should not return.
    {
        Environment.ExitCode = code;
        Process.GetCurrentProcess().Kill();
    }
#pragma warning restore CS8763 // A method marked [DoesNotReturn] should not return.

    /// <summary>
    /// Write an error message to the standard error stream with a red console foreground color and terminate the application.
    /// </summary>
    /// <param name="value">The value to write.</param>
    [DoesNotReturn]
    public static void WriteError(object? value) => WriteRawError(string.Format("Error: {0}", value));

    /// <inheritdoc cref="WriteLine(object?, ConsoleColor)"/>
    public static void WriteLine(object? value) => WriteLine(value, Console.ForegroundColor);

    /// <summary>
    /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
    /// </summary>
    /// <param name="value">The value to write.</param>
    public static void WriteLine(object? value, ConsoleColor color)
    {
        if (_verboseEnabled)
        {
            InternalWriteLine(value, color);
        }
    }

    /// <inheritdoc cref="WriteError(object?)"/>
    /// <remarks>
    /// This method prints the string as it is.
    /// </remarks>
    [DoesNotReturn]
    public static void WriteRawError(object? value)
    {
        InternalWriteError(value);
        Interrupt();
    }

    /// <inheritdoc cref="WriteWarning(object?)"/>
    public static void WriteRawWarning(object? value) => WriteLine(value, ConsoleColor.Yellow);

    /// <summary>
    /// Write a warning message to the standard output stream with a yellow console foreground color.
    /// </summary>
    /// <param name="value">The value to write.</param>
    public static void WriteWarning(object? value) => WriteRawWarning(string.Format("Warning: {0}", value));

    public CommandLine AddDescription(string description)
    {
        if (!string.IsNullOrEmpty(description))
        {
            _root.Description = description;
        }

        return this;
    }

    public CommandLine AddPrefix(string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
        {
            Prefix = prefix;
        }

        return this;
    }

    public int Invoke(IEnumerable<string> args)
    {
        foreach (var command in Parser.GetResults())
        {
            AddCommand(command);
        }

        if (!args.Any())
        {
            args = args.Append("--help");
        }

        return _cfg.Invoke(args.ToArray());
    }

    public CommandLine SelectAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(Assembly = assembly);
        return this;
    }

    private static void InternalWriteError(object? value)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(value);
        Console.ResetColor();
    }

    private static void InternalWriteLine(object? value, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ResetColor();
    }

    private void AddCommand(Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        _root.Add(command.InternalCommand);
    }
}