namespace Maid;

/// <summary>
/// Specifies an option for a command.
/// </summary>
public sealed class OptionAttribute : SymbolAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OptionAttribute"/> class with the specified name.
    /// </summary>
    /// <param name="name">The name of the option.</param>
    public OptionAttribute(string name) : base(name)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionAttribute"/> class with the specified name and description.
    /// </summary>
    /// <param name="name">The name of the option.</param>
    /// <param name="description">The description of the option.</param>
    public OptionAttribute(string name, string description) : base(name)
    {
        Description = description;
    }

    /// <summary>
    /// Gets or sets the alias for the option.
    /// </summary>
    public string? Alias
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the aliases for the option.
    /// </summary>
    public string[]? Aliases
    {
        get;
        set;
    }
}
