namespace Geronimus.Maybe.Tests;

[TestClass]
public class ValuedMaybeTests
{
    [TestMethod]
    public void GivenNull_ItsConstructorThrowsAnArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(
        #nullable disable
            () => { Maybe.ForValue<string>( null ); }
        #nullable restore
        );
    }

    [TestMethod]
    public void ValuedMaybesOfTheSameTypeAreEqual()
    {
        IMaybe<int> intEx1 = Maybe.ForValue( -1 );
        IMaybe<int> intEx2 = Maybe.ForValue( -1 );

        Assert.IsFalse( intEx1.IsEmpty );
        Assert.IsFalse( intEx2.IsEmpty );
        Assert.AreEqual( intEx2, intEx1 );

        IMaybe<string> strEx1 = Maybe.ForValue( "¡Hola!" );
        IMaybe<string> strEx2 = Maybe.ForValue( "¡Hola!");

        Assert.IsFalse( strEx1.IsEmpty );
        Assert.IsFalse( strEx2.IsEmpty );
        Assert.AreEqual( strEx2, strEx1 );
    }

    [TestMethod]
    public void GetValue_ReturnsTheContainedValue()
    {
        string val = "Hello, Muddah! Hello, Fadda!";
        IMaybe<string> ex = Maybe.ForValue( val );

        Assert.AreEqual( val, ex.GetValue() );
    }

    [TestMethod]
    public void GetValueOrElse_ReturnsTheContainedValue()
    {
        string val = "Bill Haydon";
        string defaultVal = "Percy Allelyne";
        IMaybe<string> result = Maybe.ForValue( val );

        Assert.AreEqual( val, result.GetValueOrElse( defaultVal ) );
    }

    [TestMethod]
    public void IterationCanReturnTheValue()
    {
        IMaybe<int> maybeResult = Maybe.ForValue( 42 );
        List<int> results = new();

        foreach ( int item in maybeResult )
        {
            results.Add( item );
        }

        Assert.AreEqual<int>( 1, results.Count );
        Assert.AreEqual<int>( 42, results[ 0 ] );
    }
}
