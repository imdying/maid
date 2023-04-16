namespace Maid.Test.Commands;

public sealed class Crash : Command
{
    protected internal override void Execute(object? args)
    {
        throw new NotImplementedException();
    }
}