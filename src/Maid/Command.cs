using System.CommandLine;
using System.Reflection;
using System.Text;

using Maid.Invocation;
using Maid.Parsing;
using Maid.Properties;

namespace Maid;

/// <summary>
/// Represents a specific action that the application performs.
/// </summary>
public abstract class Command
{
    private string? _name;

    protected Command()
    {
        if (string.IsNullOrWhiteSpace(Properties.Name))
        {
            Name = PascalToKebabCase(Inheritor.Name).ToLower();
        }

        #region Adds synopsis to description.
        if (string.IsNullOrWhiteSpace(Properties.Description))
        {
            Description = Resources.SymbolNoDescription;
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(Synopsis))
            {
                Description = string.Format("{0}\n\n{1}", Synopsis, Properties.Description);
            }
        } 
        #endregion

        InternalCommand = new(Name, Synopsis, Description)
        {
            Hidden = Hidden
        };

        if (IsExecuteOverridden())
        {
            InternalCommand.Action = new CommandHandler(this);
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the symbol is hidden.
    /// </summary>
    public virtual bool Hidden
    {
        get;
        set;
    }

    internal Dictionary<CliArgument, PropertyInfo>? Arguments
    {
        get;
        set;
    }

    internal string? Description
    {
        get;
        set;
    }

    internal Type Inheritor => this.GetType();

    internal CliCommand InternalCommand
    {
        get;
        set;
    }

    internal string Name
    {
        get => _name ?? throw new ArgumentNullException(nameof(Name));
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(Name));
            }

            _name = value;
        }
    }

    internal Dictionary<CliOption, PropertyInfo>? Options
    {
        get;
        private set;
    }

    internal Command? Parent
    {
        get;
        private set;
    }

    internal string? Synopsis => Properties.Synopsis;

    /// <summary>
    /// Gets or sets the properties of the command, including its name, synopsis, and description.
    /// </summary>
    /// <remarks>
    /// You can change the values of the properties to customize the behavior of the command. The <see cref="Name">Name</see> property
    /// determines the name of the command, which is used to invoke it. The <see cref="Synopsis">Synopsis</see> property is a short description
    /// of the command, which is displayed in the command-line help. The Description property is a longer description
    /// of the command, which provides more detailed information about what the command does.
    /// </remarks>
    protected virtual (string Name, string? Synopsis, string? Description) Properties
    {
        get;
    }

    internal void AddArgument(PropertyInfo property, CliSymbol symbol)
    {
        switch (symbol)
        {
            case CliOption option:
                (Options ??= new()).Add(option, property);
                break;
            case CliArgument argument:
                (Arguments ??= new()).Add(argument, property);
                break;
            default:
                throw new NotSupportedException();
        }

        InternalCommand.Add(symbol);
    }

    internal void AddCommand(Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        InternalCommand.Add(command.InternalCommand);
    }

    /// <summary>
    /// Determines whether the <see cref="Execute(object?)"/> method has been overridden in the current class or not.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if the <see cref="Execute(object?)"/> method has been overridden in the current class;
    ///   otherwise, <c>false</c>.
    /// </returns>
    internal bool IsExecuteOverridden()
    {
        var method = Execute;
        var baseMethod = method.Method;
        var derivedMethod = baseMethod.GetBaseDefinition();

        return baseMethod.DeclaringType != derivedMethod.DeclaringType;
    }

    internal void SetParent(Command? parent) => Parent = parent;

    /// <summary>
    /// Executes the command with the given arguments.
    /// </summary>
    /// <param name="args">
    /// An <see cref="IReadOnlyList{T}"/> containing the arguments passed to the parent command
    /// or/and the parent of the parent, and so on.
    /// </param>
    /// <remarks>
    /// This method serves as the starting point for <see cref="Command">command</see> execution and should be overridden by derived classes to perform the specific actions
    /// of the command. If the command doesn't require any arguments, then the <paramref name="args"/>
    /// parameter can be ignored.
    /// </remarks>
    protected internal virtual void Execute(object? args) { }

    /// <summary>
    /// Converts a Pascal case string to kebab case.
    /// </summary>
    /// <param name="value">The Pascal case string to convert.</param>
    /// <returns>The kebab case string.</returns>
    private static string PascalToKebabCase(string value)
    {
        var result = new StringBuilder();

        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];

            if (i > 0 && char.IsUpper(c))
            {
                result.Append('-');
            }

            result.Append(char.ToLower(c));
        }

        return result.ToString();
    }
}