namespace Maid;

/// <summary>
/// Defines the arity of an option or argument.
/// </summary>
/// <remarks>
/// The arity refers to the number of values that can be passed on the command line.
/// </remarks>
public enum ArgumentArity : uint
{
    /// <summary>
    /// The default value defined in the System.CommandLine library.
    /// </summary>
    Default,

    /// <summary>
    /// An arity that does not allow any values.
    /// </summary>
    Zero,

    /// <summary>
    /// An arity that may have one value, but no more than one.
    /// </summary>
    ZeroOrOne,

    /// <summary>
    /// An arity that must have exactly one value.
    /// </summary>
    ExactlyOne,

    /// <summary>
    /// An arity that may have multiple values.
    /// </summary>
    ZeroOrMore,

    /// <summary>
    /// An arity that must have at least one value.
    /// </summary>
    OneOrMore
}