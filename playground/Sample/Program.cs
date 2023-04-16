namespace Maid.Sample;

internal class Program
{
    static int Main(string[] args) => new CommandLine().AddPrefix("--")
                                                       .AddDescription("A sample application.")
                                                       .Invoke(args);
}
