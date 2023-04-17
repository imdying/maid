namespace Maid.Sample;

internal class Program
{
    static int Main(string[] args) => new CommandLineBuilder().WithPrefix("--")
                                                              .WithDescription("A sample application.")
                                                              .Build(args);
}
