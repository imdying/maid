using Maid.Properties;

namespace Maid;

/// <summary>
/// A symbol defining a value that can be passed on the command line to a <see cref="Command">command</see> or <see cref="OptionAttribute">option</see>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public abstract class SymbolAttribute : Attribute
{
    private string? _description;

    protected SymbolAttribute(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        Name = name;
    }

    /// <summary>
    /// Defines the arity of the argument.
    /// </summary>
    public ArgumentArity Arity
    {
        get;
        set;
    }

    public string? Description
    {
        get => _description ??= Resources.SymbolNoDescription;
        set => _description = value;
    }

    public bool Hidden
    {
        get;
        set;
    }

    public string Name
    {
        get;
        set;
    }
}
