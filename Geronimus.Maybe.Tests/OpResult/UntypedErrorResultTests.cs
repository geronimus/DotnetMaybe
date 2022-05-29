namespace Geronimus.Maybe.Tests;

[TestClass]
public class UntypedErrorResultTests
{
    [TestMethod]
    public void ErrorResult_RemainsUsableEvenWhenBadlyInitialized()
    {
    #nullable disable
        IOpResult badExample = OpResult.Error( null );
    #nullable restore

        Assert.IsTrue( badExample.IsFailure );
        Assert.IsFalse( badExample.HasError ); // Indicates that the error was
                                               // not correctly initialized, but
                                               // does not cause the instance to
                                               // malfunction.
        Assert.IsFalse( badExample.IsSuccess );
        Assert.IsTrue( badExample is IErrorResult );
        Assert.IsTrue( badExample is IEmptyValue );
        Assert.IsInstanceOfType(
            badExample.Error,
            typeof( ErrorNotProvidedException )
        );
    }

    [TestMethod]
    public void ErrorResult_FunctionsAsExpectedWhenWellInitialized()
    {
        string myMessage = "Hands off!";
        Exception myError = new AccessViolationException( myMessage );
        IOpResult example = OpResult.Error( myError );

        Assert.IsTrue( example.IsFailure );
        Assert.IsTrue( example.HasError );
        Assert.IsFalse( example.IsSuccess );
        Assert.IsTrue( example is IErrorResult );
        Assert.IsTrue( example is IEmptyValue );
        Assert.IsTrue( example.Error is AccessViolationException );
        Assert.AreEqual<Exception>( myError, example.Error );
        Assert.AreEqual( myMessage, example.Error.Message );
    }
}
