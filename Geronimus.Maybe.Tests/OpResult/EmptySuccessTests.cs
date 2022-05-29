namespace Geronimus.Maybe.Tests;

[TestClass]
public class EmptySuccessTests
{
    [TestMethod]
    public void EmptySuccess_PropertiesBehaveAsExpected()
    {
        IOpResult example = OpResult.Success();

        Assert.IsFalse( example.IsFailure );
        Assert.IsFalse( example.HasError );
        Assert.IsTrue( example.IsSuccess );

        Assert.ThrowsException<InvalidOperationException>(
            () => { var error = example.Error; }
        );
    }

    [TestMethod]
    public void EmptySuccess_AllInstancesAreEqual()
    {
        IOpResult ex1 = new EmptySuccess();
        IOpResult ex2 = new EmptySuccess();

        Assert.AreEqual<IOpResult>( ex2, ex1 );
    }

    public void EmptySuccess_OpResultStaticInitializerProvidesSameInstance()
    {
        IOpResult ex1 = OpResult.Success();
        IOpResult ex2 = OpResult.Success();

        Assert.AreSame( ex2, ex1 );
        Assert.IsTrue( ex1 == ex2 );
        // Strange! I didn't implement this behaviour explicitly. It must be a
        // compiler optimization.
    }
}