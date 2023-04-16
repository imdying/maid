using System.Reflection;

namespace Maid;

public interface ICommandLineFluentInterface<T>
{
    T AddDescription(string description);

    int Invoke(IEnumerable<string> args);

    T SelectAssembly(Assembly assembly);

    T AddPrefix(string prefix);
}
