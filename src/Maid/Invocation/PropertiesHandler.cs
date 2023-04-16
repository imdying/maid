using System.CommandLine;
using System.Reflection;

using Maid.Parsing;

namespace Maid.Invocation;

internal sealed class PropertiesHandler
{
    private readonly Command _command;

    public PropertiesHandler(Command command)
    {
        _command = command;
    }

    public IReadOnlyList<object?> GetParamaters(ParseResult parseResult)
    {
        SetParameters(parseResult);
        return GetArgumentsFromParent(_command, parseResult);
    }

    /// <summary>
    /// Returns all arguments defined in the parent hierarchy of the current parse result,
    /// starting from the top-level parent and working down to the current parse result.
    /// </summary>
    /// <returns>An enumerable collection of argument values, in the order they were added.</returns>
    private static IReadOnlyList<object?> GetArgumentsFromParent(Command command, ParseResult parseResult)
    {
        var arguments = new List<object?>();

        // Traverse up the parent hierarchy and collect arguments
        var current = command.Parent;

        while (current is not null)
        {
            if (current.Arguments is not null)
            {
                foreach (var argument in current.Arguments.Keys)
                {
                    var result = parseResult.FindResultFor(argument);

                    if (result is not null)
                    {
                        arguments.Add(result.GetValueOrDefault<object>());
                    }
                }
            }
            current = current.Parent;
        }

        // Return collected arguments in reverse order (from parent to child)
        arguments.Reverse();

        return arguments;
    }

    private void SetParameters(ParseResult parseResult)
    {
        if (_command.Arguments is not null)
        {
            foreach (var argument in _command.Arguments)
            {
                SetValue(
                    argument.Value, 
                    parseResult.GetValue<object>(argument.Key.Name)
                );
            }
        }

        if (_command.Options != null)
        {
            foreach (var option in _command.Options)
            {
                SetValue(
                    option.Value,
                    parseResult.GetValue<object>(option.Key.Name)
                );
            }
        }
    }

    private void SetValue(PropertyInfo property, object? value) => SetValue(_command, property, value);

    internal static void SetValue(object target, PropertyInfo property, object? value)
    {
        ArgumentNullException.ThrowIfNull(property);

        if (!property.CanWrite)
        {
            throw new InvalidOperationException(
                $"Cannot write to property at '{property.DeclaringType}.{property.Name}'."
            );
        }

        if (value is not null && !ValueMatchUnderlyingType(property.PropertyType, value.GetType()))
        {
            throw new InvalidCastException(
                $"Type mismatch. Cannot convert '{property.PropertyType}' to '{value.GetType()}'."
            );
        }

        // Preserve the default value of the property if the param 'value' is null.
        if (value is not null)
        {
            property.SetValue(target, value);
        }
    }

    private static bool ValueMatchUnderlyingType(Type source, Type target)
    {
        if (IsNullable(source, out var underlyingType) && underlyingType == target)
            return true;

        if (source == target)
            return true;

        return false;
    }

    private static bool IsNullable(Type type, out Type? baseType) => (baseType = Nullable.GetUnderlyingType(type)) != null;
}
