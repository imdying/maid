namespace Maid.Sdk;

/// <summary>
/// Provides a way for developers to control the environment of a command.
/// </summary>
public abstract class Flex
{
    /// <summary>
    /// Gets a value indicating whether the command has outputted anything before.
    /// </summary>
    /// <remarks>This only applies to <see cref="WriteLine(object?)"/> and etc.</remarks>
    public bool HasWritten
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets or sets a <see cref="bool"/> value that controls the visibility of outputs from <see cref="WriteLine(object?)"/> and similar methods.
    /// </summary>
    public virtual bool IsQuiet
    {
        get;
        set;
    }

    /// <summary>
    /// Gets a value indicating whether the command is verbose or not.
    /// </summary>
    private bool IsVerbose => IsQuiet is false;

    /// <summary>
    /// Writes an error message to the console.
    /// </summary>
    /// <param name="value">The error message to write.</param>
    public void WriteError(object? value) => CommandLine.WriteError(value);

    /// <summary>
    /// Writes a message to the console.
    /// </summary>
    /// <param name="value">The message to write.</param>
    public void WriteLine(object? value)
    {
        if (IsVerbose)
        {
            ValidateWritten();
            Console.WriteLine(value);
        }
    }

    /// <summary>
    /// Writes a warning message to the console.
    /// </summary>
    /// <param name="value">The warning message to write.</param>
    public void WriteWarning(object? value)
    {
        if (IsVerbose)
        {
            ValidateWritten();
            CommandLine.WriteWarning(value);
        }
    }

    /// <summary>
    /// Validates whether the command has outputted anything before.
    /// </summary>
    private void ValidateWritten()
    {
        if (!HasWritten)
        {
            HasWritten = true;
        }
    }
}