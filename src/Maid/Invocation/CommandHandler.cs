using System.CommandLine;

namespace Maid.Invocation;

internal sealed class CommandHandler : CliAction
{
    private readonly Command _command;
    private readonly PropertiesHandler _propertiesHandler;
    private readonly Thread _thread;

    public CommandHandler(Command command)
    {
        _command = command;
        _thread = new Thread(command.Execute);
        _propertiesHandler = new(command);
    }

    public override int Invoke(ParseResult parseResult)
    {
        try
        {
            _thread.Start(
                _propertiesHandler.GetParamaters(parseResult)
            );
        }
        catch (Exception)
        {
            Environment.ExitCode = -1;
            throw;
        }

        return 0;
    }

    public override Task<int> InvokeAsync(ParseResult parseResult, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
