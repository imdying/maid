namespace Maid.Sample;

internal class Program
{
    static int Main(string[] args)
    {
        var commandLine = new CommandLine(args)
        {
            Description = "A sample application."
        };

        return commandLine.Invoke();
    }
}
