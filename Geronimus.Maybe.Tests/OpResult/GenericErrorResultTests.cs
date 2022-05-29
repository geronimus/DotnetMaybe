namespace Geronimus.Maybe.Tests;

[TestClass]
public class GenericErrorResultTests
{
    [TestMethod]
    public void ErrorResult_FunctionsAsExpectedWhenTheErrorIsNotInitialized()
    {
    #nullable disable
        IOpResult<int> badInit = OpResult.Error<int>( null );
    #nullable restore

        Assert.IsTrue( badInit.IsFailure );
        Assert.IsFalse( badInit.IsSuccess );
        Assert.IsFalse( badInit.HasError );
        Assert.IsFalse( badInit.HasResult );

        Assert.IsInstanceOfType(
            badInit.Error,
            typeof( ErrorNotProvidedException )
        );

        Assert.ThrowsException<ErrorNotProvidedException>(
            () => { var res = badInit.Result; }
        );

        int defaultResult = 42;
        Assert.AreEqual<int>(
            defaultResult,
            badInit.GetResultOrElse( defaultResult )
        );

        List<int> results = new(
            (
                from int item in badInit
                select item
            )
        );

        Assert.AreEqual<int>( 0, results.Count );
    }

    [TestMethod]
    public void ErrorResult_FunctionsAsExpectedWhenTheErrorIsWellInitialized()
    {
        string myMessage = "No touchy!";
        AccessViolationException myError =
            new AccessViolationException( myMessage );
        IOpResult<int> example = OpResult.Error<int>( myError );

        Assert.IsTrue( example.IsFailure );
        Assert.IsFalse( example.IsSuccess );
        Assert.IsTrue( example.HasError );
        Assert.IsFalse( example.HasResult );

        Assert.IsInstanceOfType(
            example.Error,
            typeof( AccessViolationException )
        );

        Assert.ThrowsException<AccessViolationException>(
            () => { var res = example.Result; }
        );

        int defaultResult = 42;
        Assert.AreEqual<int>(
            defaultResult,
            example.GetResultOrElse( defaultResult )
        );

        List<int> results = new(
            (
                from int item in example
                select item
            )
        );

        Assert.AreEqual<int>( 0, results.Count );
    }

    [TestMethod]
    public void ErrorResult_InstancesWithIdenticalErrorsAreNotEqual()
    {
        string myMessage = "Back off! Get your own sandwich!";
        AccessViolationException myError =
            new AccessViolationException( myMessage );
        IOpResult<DateTime> ex1 = OpResult.Error<DateTime>( myError );
        IOpResult<DateTime> ex2 = OpResult.Error<DateTime>( myError );

        Assert.AreNotEqual<IOpResult<DateTime>>( ex2, ex1 );
        Assert.AreNotSame( ex2, ex1 );
    }
}
