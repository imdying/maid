using System.CommandLine;
using System.Reflection;

using InternalArgumentArity = System.CommandLine.ArgumentArity;

namespace Maid.Parsing;

internal static class ArgumentParser
{
    /// <summary>
    ///  Parses a command line symbol from a specified property.
    /// </summary>
    public static CliSymbol Parse<T>(this CommandLineParser parser, PropertyInfo property) where T : SymbolAttribute
    {
        SymbolAttribute? attribute;

        if ((attribute = property.GetCustomAttribute<T>()) is null)
        {
            throw new ArgumentNullException(nameof(attribute));
        }

        switch (attribute)
        {
            case ArgumentAttribute argument:
                break;

            case OptionAttribute option:
                option.Name = string.Format("{0}{1}", parser._builder.Prefix, attribute.Name);
                break;

            default:
                throw new NotSupportedException();
        }

        if (Argument<T>.Create(property.PropertyType, attribute.Name) is not CliSymbol symbol)
        {
            throw new InvalidCastException();
        }

        ApplyValues(symbol, attribute);

        return symbol;
    }

    /// <summary>
    /// Applies the values from a <see cref="SymbolAttribute"/> to a <see cref="CliSymbol"/> object.
    /// </summary>
    private static void ApplyValues<T>(this CliSymbol symbol, T attribute) where T : SymbolAttribute
    {
        symbol.Hidden = attribute.Hidden;
        symbol.Description = attribute.Description;

        switch (symbol)
        {
            case CliArgument argument:
            {
                if (attribute is not ArgumentAttribute data)
                {
                    throw new ArgumentException(nameof(symbol));
                }

                argument.HelpName = data.HelpName;
                argument.Arity = ConvertArgumentArity(attribute.Arity);

                break;
            }

            case CliOption option:
            {
                if (attribute is not OptionAttribute data)
                {
                    throw new ArgumentException(nameof(symbol));
                }
                
                option.Arity = ConvertArgumentArity(attribute.Arity);

                #region Aliases
                if (!string.IsNullOrWhiteSpace(data.Alias))
                {
                    option.Aliases.Add(data.Alias);
                }

                for (int i = 0; i < data.Aliases?.Length; i++)
                {
                    var value = data.Aliases[i];

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        option.Aliases.Add(value);
                    }
                }
                #endregion

                break;
            }

            default:
                throw new NotSupportedException();
        }
    }

    private static InternalArgumentArity ConvertArgumentArity(ArgumentArity arity)
    {
        return arity switch
        {
            ArgumentArity.Zero => InternalArgumentArity.Zero,
            ArgumentArity.ZeroOrOne => InternalArgumentArity.ZeroOrOne,
            ArgumentArity.ExactlyOne => InternalArgumentArity.ExactlyOne,
            ArgumentArity.ZeroOrMore => InternalArgumentArity.ZeroOrMore,
            ArgumentArity.OneOrMore => InternalArgumentArity.OneOrMore,
            _ => default,
        };
    }
}
