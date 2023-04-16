namespace Maid.Test.Units;

[TestClass]
public class Command_Execution
{
    [TestMethod]
    public void Thread_Should_Not_Block_Environment_Exit()
    {
        const string command = nameof(Commands.EnvironmentExit);
        Assert.IsTrue(Internal.InvokeProcess(command) == 0);
        Assert.IsTrue(Internal.InvokeProcess(string.Format("{0} 0", command)) == 0);
        Assert.IsTrue(Internal.InvokeProcess(string.Format("{0} 1", command)) == 1);
        Assert.IsTrue(Internal.InvokeProcess(string.Format("{0} -69", command)) == -69);
    }

    [TestMethod]
    public void Should_Return_Non_Zero_If_Command_Doesnt_Exist()
    {
        Assert.IsTrue(Internal.InvokeProcess(nameof(Should_Return_Non_Zero_If_Command_Doesnt_Exist)) != 0);
    }
}