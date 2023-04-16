namespace Maid.Test.Commands;

public sealed class EnvironmentExit : Command
{
    [Argument(
        1, 
        "Code", 
        "The exit code to return.", 
        Arity = ArgumentArity.ZeroOrOne
    )]
    public int Code
    {
        get;
        set;
    }

    protected override (string Name, string? Synopsis, string? Description) Properties => (nameof(EnvironmentExit), "", "");

    protected internal override void Execute(object? args)
    {
        Environment.Exit(Code);
    }
}