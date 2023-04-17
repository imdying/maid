namespace Sample.Commands;

public sealed class Greet : Command
{
    [Argument(
        1, 
        "user",
        Arity = ArgumentArity.ExactlyOne
    )]
    public string? User
    {
        get;
        set;
    }

    protected override void Execute(object? args)
    {
        Console.WriteLine("You said your greetings to {0}.", User);
    }
}