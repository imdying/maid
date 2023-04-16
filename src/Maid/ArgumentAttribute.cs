namespace Maid;

public sealed class ArgumentAttribute : SymbolAttribute
{
    public ArgumentAttribute(uint id, string name) : base(name)
    {
        this.Index = id;
    }

    public ArgumentAttribute(uint id, string name, string description) : base(name)
    {
        this.Index = id;
        this.Description = description;
    }

    /// <summary>
    /// The name used in help output to describe the argument. 
    /// </summary>
    public string? HelpName
    {
        get;
        set;
    }

    internal uint Index
    {
        get;
        private set;
    }
}