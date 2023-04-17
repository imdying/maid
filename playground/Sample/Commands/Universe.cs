namespace Sample.Commands;

public sealed class Universe : Command
{
    public sealed class Reset : Command
    {
        protected override void Execute(object? args)
        {
            Console.WriteLine("The universe has been reset.");
        }
    }

    public sealed class Create : Command
    {
        protected override void Execute(object? args)
        {
            Console.WriteLine("BANG");
        }
    }
}