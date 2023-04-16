namespace Sample.Commands;

public sealed class Hello : Command
{
    protected override (string Name, string? Synopsis, string? Description) Properties => (
        Name:
        string.Empty, // If the name is null, Maid uses the class name instead.

        Synopsis:
        "This is a brief description.",

        Description:
        "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.\n" +
        "Vestibulum tortor quam, feugiat vitae, ultricies eget, tempor sit amet, ante. Donec eu libero sit amet quam egestas semper. Aenean ultricies mi vitae est. Mauris placerat eleifend leo."
    );

    protected override void Execute(object? args)
    {
        Console.WriteLine("Hello World");
    }
}