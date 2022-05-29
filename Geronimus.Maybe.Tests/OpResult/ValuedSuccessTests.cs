namespace Geronimus.Maybe.Tests;

[TestClass]
public class ValuedSuccessTests
{
    [TestMethod]
    public void ValuedSucces_ThrowsArgumentNullExceptionWhenNoValueProvided()
    {
        Assert.ThrowsException<ArgumentNullException>(
        #nullable disable
            () => { OpResult.Success<System.Data.SqlTypes.SqlBytes>( null ); }
        #nullable restore
        );
    }

    [TestMethod]
    public void ValuedSucces_WhenWellInitialized_FunctionsAsExpected()
    {
        string myVal =
            "I'd rather be using Scala, but it has no decent UI library.";
        
        IOpResult<string> example = OpResult.Success( myVal );

        Assert.IsFalse( example.IsFailure );
        Assert.IsFalse( example.HasError );
        Assert.IsTrue( example.IsSuccess );
        Assert.IsTrue( example.HasResult );

        Assert.ThrowsException<InvalidOperationException>(
            () => { var err = example.Error; }
        );

        Assert.AreEqual( myVal, example.Result );

        string replacer = "C# will do just fine, Sir!";

        Assert.AreEqual( myVal, example.GetResultOrElse( replacer ) );

        List<string> results = new (
            (
                from string item in example
                select item
            )
        );

        Assert.AreEqual<int>( 1, results.Count );
        Assert.AreEqual( myVal, results[ 0 ] );
    }

    [TestMethod]
    public void ValuedResult_IdenticalResultsAreEqual()
    {
        string deanoResult = "Hey pally.. Which way's the audience?";
        IOpResult<string> ex1 = OpResult.Success( deanoResult );
        IOpResult<string> ex2 = OpResult.Success( deanoResult );

        Assert.AreEqual<IOpResult<string>>( ex2, ex1 );
    }
}
