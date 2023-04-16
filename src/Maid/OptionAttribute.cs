namespace Maid;

public sealed class OptionAttribute : SymbolAttribute
{
    public OptionAttribute(string name) : base(name)
    {
    }

    public OptionAttribute(string name, string description) : base(name)
    {
        Description = description;
    }

    public string? Alias
    {
        get;
        set;
    }

    public string[]? Aliases
    {
        get;
        set;
    }
}
