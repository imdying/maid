using System.Reflection;

namespace Maid.Parsing;

internal sealed class CommandLineParser
{
    public readonly CommandLineBuilder _builder;

    public CommandLineParser(CommandLineBuilder builder)
    {
        _builder = builder;
    }

    public IEnumerable<Command> GetResults()
    {
        var results = new List<Command>();
        var classes = FilterByCommand(_builder.Assembly.GetExportedTypes()).Where(t => !t.IsNested);

        foreach (var @class in classes)
        {
            var command = Parse(@class, null);
            results.Add(command);
        }

        return results;
    }

    public Command Parse(Type reference, Command? parent)
    {
        var command  = CreateInstance(reference);
        var children = FilterByCommand(reference.GetNestedTypes());

        ParseArguments(command);

        if (parent is not null)
        {
            command.SetParent(parent);
        }

        if (children.Any())
        {
            parent = command;
        }

        foreach (var child in children)
        {
            if (child.IsNested)
            {
                command.AddCommand(
                    Parse(child, parent)
                );
            }
        }

        return command;
    }

    private static Command CreateInstance(Type type)
    {
        var instance = Activator.CreateInstance(type);

        if (instance is null || instance is not Command command)
        {
            throw new InvalidOperationException();
        }

        return command;
    }

    private static IEnumerable<Type> FilterByCommand(IEnumerable<Type> types) 
        => types.Where(t => typeof(Command).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

    /// <summary>
    /// Sorts the properties that have the <see cref="ArgumentAttribute"/> and adds them to the Arguments dictionary in the sorted order.
    /// </summary>
    private static PropertyInfo[] SortArguments(PropertyInfo[] props)
    {
        var arguments = props.Where(prop => Attribute.IsDefined(prop, typeof(ArgumentAttribute)));
        var orderedBy = arguments.OrderBy(prop =>
        {
            var attribute = prop.GetCustomAttribute<ArgumentAttribute>();
            return attribute is null ? throw new ArgumentNullException(nameof(prop)) : attribute.Index;
        });

        return orderedBy.ToArray();
    }

    private void ParseArguments(Command command)
    {
        var props = command.Inheritor.GetProperties();

        foreach (var prop in SortArguments(props))
        {
            command.AddArgument(prop, this.Parse<ArgumentAttribute>(prop));
        }

        foreach (var prop in props)
        {
            if (Attribute.IsDefined(prop, typeof(OptionAttribute)))
            {
                command.AddArgument(prop, this.Parse<OptionAttribute>(prop));
            }
        }
    }
}
