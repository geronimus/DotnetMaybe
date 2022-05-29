namespace Geronimus.Maybe.Tests;

[TestClass]
public class EmptyMaybeTests
{
    [TestMethod]
    public void EmptyMaybesOfTheSameType_AreEqual()
    {
        var bool1 = Maybe.Empty<bool>();
        var bool2 = Maybe.Empty<bool>();

        Assert.IsTrue( bool1.IsEmpty );
        Assert.IsTrue( bool2.IsEmpty );
        Assert.IsTrue( bool1.Equals( bool2 ) );

        var dt1 = Maybe.Empty<DateTime>();
        var dt2 = Maybe.Empty<DateTime>();
        
        Assert.IsTrue( dt1.IsEmpty );
        Assert.IsTrue( dt2.IsEmpty );
        Assert.IsTrue( dt1.Equals( dt2 ) );
    }

    [TestMethod]
    public void GetValue_ThrowsInvalidOperationException()
    {
        Assert.ThrowsException<InvalidOperationException>(
            () => { var item = Maybe.Empty<string>().GetValue(); }
        );
    }

    [TestMethod]
    public void GetValueOrElse_ReturnsTheDefaultValue()
    {
        string defaultVal = "Hello, World!";

        Assert.AreEqual<string>(
            defaultVal,
            Maybe.Empty<string>().GetValueOrElse( defaultVal )
        );
    }

    [TestMethod]
    public void IterationReturnsNothing()
    {
        IMaybe<int> example = Maybe.Empty<int>();
        List<int> results = new();

        foreach ( int item in example )
        {
            results.Add( item );
        }
    }
}