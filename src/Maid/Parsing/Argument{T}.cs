using System.CommandLine;

namespace Maid.Parsing;

internal static class Argument<T> where T : SymbolAttribute
{
    public static CliSymbol Create(Type type, string name)
    {
        var symbolType = GetAppropriateType();
        var genericType = symbolType.MakeGenericType(type);
        var instance = Activator.CreateInstance(genericType, new object?[] { name });

        if (instance is not CliSymbol symbol)
        {
            throw new InvalidCastException();
        }

        return symbol;
    }

    private static Type GetAppropriateType()
    {
        if (typeof(T) == typeof(OptionAttribute))
        {
            return typeof(CliOption<>);
        }
        else if (typeof(T) == typeof(ArgumentAttribute))
        {
            return typeof(CliArgument<>);
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}