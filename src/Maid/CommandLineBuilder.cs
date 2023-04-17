using System.CommandLine;
using System.Reflection;
using System.Text;

using Maid.Parsing;
using Maid.Properties;

namespace Maid;

/// <summary>
/// Provides a builder for creating and executing the command line interpreter.
/// </summary>
public sealed class CommandLineBuilder
{
    private readonly CliConfiguration _cfg;
    private readonly CliRootCommand _root;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandLineBuilder"/> class.
    /// </summary>
    public CommandLineBuilder()
    {
        _root = new(Resources.SymbolNoDescription);
        _cfg = new(_root);
        Parser = new(this);
    }

    /// <summary>
    /// Gets or sets the <see cref="Assembly">assembly</see> that contains the <see cref="Command"/> objects.
    /// </summary>
    /// <remarks>
    /// The default value is the calling assembly, as returned by <see cref="Assembly.GetCallingAssembly"/>.
    /// </remarks>
    internal Assembly Assembly { get; private set; } = Assembly.GetCallingAssembly();

    /// <summary>
    /// Gets or sets the <see cref="CommandLineParser"/> used to parse the command line.
    /// </summary>
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
    /// Builds and executes the command line interpreter.
    /// </summary>
    /// <param name="args">The command line arguments to be parsed and executed.</param>
    /// <returns>The exit code of the command line interpreter.</returns>
    public int Build(IEnumerable<string> args)
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

    /// <summary>
    /// Sets the <see cref="Assembly">assembly</see> that contains the <see cref="Command"/> objects.
    /// </summary>
    /// <param name="assembly">The assembly to use.</param>
    /// <returns>The current instance of the <see cref="CommandLineBuilder"/> for method chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is null.</exception>
    public CommandLineBuilder WithAssembly(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(Assembly = assembly);
        Assembly = assembly;
        return this;
    }

    /// <summary>
    /// Sets the description of the command line.
    /// </summary>
    /// <param name="value">The description of the command line.</param>
    /// <returns>The current instance of the <see cref="CommandLineBuilder"/> for method chaining.</returns>
    public CommandLineBuilder WithDescription(string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _root.Description = value;
        }

        return this;
    }

    /// <summary>
    /// Sets the prefix used to indicate command line options.
    /// </summary>
    /// <param name="value">The prefix used to indicate command line options.</param>
    /// <returns>The current instance of the <see cref="CommandLineBuilder"/> for method chaining.</returns>
    public CommandLineBuilder WithPrefix(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            Prefix = value;
        }

        return this;
    }

    /// <summary>
    /// Sets the input and output encoding of the console to UTF-8 encoding.
    /// </summary>
    /// <returns>The current instance of the <see cref="CommandLineBuilder"/> for method chaining.</returns>
    public CommandLineBuilder WithUTF8Encoding()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        return this;
    }

    /// <summary>
    /// Adds a command to the root command.
    /// </summary>
    /// <param name="command">The command to add.</param>
    private void AddCommand(Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        _root.Add(command.InternalCommand);
    }
}
