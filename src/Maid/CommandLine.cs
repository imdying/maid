using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Maid;

/// <summary>
/// Provides higher-level services (static methods) for common command line interface tasks.
/// </summary>
public static class CommandLine
{
    private static bool _verboseEnabled = true;

    /// <summary>
    /// Interrupts the execution of the command line interface.
    /// </summary>
    [DoesNotReturn]
    public static void Interrupt() => Terminate(-1);

    /// <summary>
    /// Sets the verbose flag of the command line interface.
    /// </summary>
    /// <param name="verboseEnabled">A boolean value indicating whether the verbose flag is enabled.</param>
    public static void SetVerbose(bool verboseEnabled) => _verboseEnabled = verboseEnabled;

    /// <summary>
    /// Terminates the execution of the command line interface with the specified exit code.
    /// </summary>
    /// <param name="code">An integer value representing the exit code.</param>
    [DoesNotReturn]
    public static void Terminate(int code)
#pragma warning disable CS8763 // A method marked [DoesNotReturn] should not return.
    {
        Environment.ExitCode = code;
        Process.GetCurrentProcess().Kill();
    }
#pragma warning restore CS8763 // A method marked [DoesNotReturn] should not return.

    /// <summary>
    /// Writes the specified value to the standard error stream of the command line interface.
    /// </summary>
    /// <param name="value">The value to write.</param>
    [DoesNotReturn]
    public static void WriteError(object? value) => WriteRawError(string.Format("Error: {0}", value));

    /// <inheritdoc cref="WriteLine(object?, ConsoleColor)"/>
    public static void WriteLine(object? value) => WriteLine(value, Console.ForegroundColor);

    /// <inheritdoc cref="InternalWriteLine(object?, ConsoleColor)"/>
    public static void WriteLine(object? value, ConsoleColor color)
    {
        if (_verboseEnabled)
        {
            InternalWriteLine(value, color);
        }
    }

    /// <inheritdoc cref="WriteError(object?)"/>
    [DoesNotReturn]
    public static void WriteRawError(object? value)
    {
        InternalWriteError(value);
        Interrupt();
    }

    /// <inheritdoc cref="WriteWarning(object?)"/>
    public static void WriteRawWarning(object? value) => WriteLine(value, ConsoleColor.Yellow);

    /// <summary>
    /// Writes the specified value to the standard output stream of the command line interface, prepending it with "Warning: ".
    /// </summary>
    /// <param name="value">The value to write.</param>
    public static void WriteWarning(object? value) => WriteRawWarning(string.Format("Warning: {0}", value));

    /// <summary>
    /// Writes the specified value to the standard error stream of the command line interface using the specified color.
    /// </summary>
    /// <param name="value">The value to write.</param>
    private static void InternalWriteError(object? value)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(value);
        Console.ResetColor();
    }

    /// <summary>
    /// Writes the specified value to the standard output stream of the command line interface.
    /// </summary>
    /// <param name="value">The value to write.</param>
    /// <param name="color">The color to use.</param>
    private static void InternalWriteLine(object? value, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ResetColor();
    }
}