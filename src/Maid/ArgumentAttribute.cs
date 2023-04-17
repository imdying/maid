namespace Maid;

/// <summary>
/// Specifies an argument for a command.
/// </summary>
public sealed class ArgumentAttribute : SymbolAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentAttribute"/> class with the specified identifier and name.
    /// </summary>
    /// <param name="id">The zero-based position of the argument.</param>
    /// <param name="name">The name of the argument.</param>
    public ArgumentAttribute(uint id, string name) : base(name)
    {
        this.Index = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentAttribute"/> class with the specified identifier, name, and description.
    /// </summary>
    /// <param name="id">The zero-based position of the argument.</param>
    /// <param name="name">The name of the argument.</param>
    /// <param name="description">The description of the argument.</param>
    public ArgumentAttribute(uint id, string name, string description) : base(name)
    {
        this.Index = id;
        this.Description = description;
    }

    /// <summary>
    /// Gets or sets the name used in help output to describe the argument.
    /// </summary>
    public string? HelpName
    {
        get;
        set;
    }

    /// <summary>
    /// Gets the zero-based position of the argument.
    /// </summary>
    internal uint Index
    {
        get;
        private set;
    }
}