using Maid.Invocation;
using Maid.Test.Commands;

namespace Maid.Test.Units;

[TestClass]
public class Class_Property_SetValue
{
    public int? Prop
    {
        get;
        set;
    }

    public string? Prop_Should_Not_Be_Written_To
    {
        get;
    }

    public string? Prop_Has_Default_Value { get; set; } = nameof(Prop_Has_Default_Value);

    [TestMethod]
    public void Prop_Should_Be_Null()
    {
        SetValue(nameof(Prop), null);
        Assert.AreEqual(null, Prop);
    }

    [TestMethod]
    public void Prop_Should_Have_Default_Value()
    {
        SetValue(nameof(Prop_Has_Default_Value), null);
        Assert.AreEqual(nameof(Prop_Has_Default_Value), Prop_Has_Default_Value);
    }

    [TestMethod]
    public void Prop_Should_Have_Value()
    {
        const int value = 777;

        SetValue(nameof(Prop), value);
        Assert.AreEqual(value, Prop);
    }

    [TestMethod]
    public void Should_Throw_If_A_Bad_Value_Is_Set_To_Prop()
    {
        Assert.ThrowsException<InvalidCastException>(() => SetValue(nameof(Prop), new object()));
    }

    [TestMethod]
    public void Should_Throw_If_Get_Setter_Only_Is_Written_To()
    {
        Assert.ThrowsException<InvalidOperationException>(() => SetValue(nameof(Prop_Should_Not_Be_Written_To), null));
    }

    private void SetValue(string propName, object? value)
        => PropertiesHandler.SetValue(this, GetType().GetProperty(propName)!, value);
}