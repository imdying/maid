using Maid.Properties;

namespace Maid;

/// <summary>
///  Provides a base class for defining a value that can be passed on the command line to a <see cref="Command">command</see>.
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
    /// Gets or sets the arity of the argument.
    /// </summary>
    public ArgumentArity Arity
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the description of the symbol.
    /// </summary>
    public string? Description
    {
        get => _description ??= Resources.SymbolNoDescription;
        set => _description = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the symbol is hidden.
    /// </summary>
    public bool Hidden
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the name of the symbol.
    /// </summary>
    public string Name
    {
        get;
        set;
    }
}
