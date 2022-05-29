using System.Collections;

namespace Geronimus.Maybe;

public interface IMaybe<T> : IEnumerable<T>, IEquatable<IMaybe<T>>
{
    public bool IsEmpty { get; }

    public T GetValue();

    public T GetValueOrElse( T defaultValue );
}

public static class Maybe
{
    public static IMaybe<T> Empty<T>() => new EmptyMaybe<T>();
    
    public static IMaybe<T> ForValue<T>( T value ) =>
        new ValuedMaybe<T>( value );
}

public class EmptyMaybe<T> : IMaybe<T>, IEmptyValue
{
    // Constructor:
    public EmptyMaybe() {}
    
    // Properties:
    public bool IsEmpty => true;

    // Methods:
    public bool Equals( IMaybe<T>? that ) =>
        that != null && that.IsEmpty;

    public override bool Equals( object? obj ) =>
        obj is IMaybe<T> that && this.Equals( that );

    public T GetValue()
    {
        throw new InvalidOperationException(
            $"You cannot get the value of an empty { nameof( IMaybe<T> ) }."
        );
    }

    public T GetValueOrElse( T defaultValue ) => defaultValue;

    public IEnumerator<T> GetEnumerator() { yield break; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override int GetHashCode() => 0;
}

public class ValuedMaybe<T> : IMaybe<T>, IValueHolder
{
    // Fields:
    private T _value;

    // Constructor:
    public ValuedMaybe( T value )
    {
        if ( value == null )
            throw new ArgumentNullException( nameof( value ) );
        else
        {
            _value = value;
        }
    }

    // Properties:
    public bool Equals( IMaybe<T>? that ) =>
        that != null && !that.IsEmpty && _value!.Equals( that.GetValue() );

    public override bool Equals( object? obj ) =>
        obj is IMaybe<T> that && this.Equals( that );

    public bool IsEmpty => false;

    // Methods:
    public T GetValue() => _value;

    public T GetValueOrElse( T defaultValue ) => _value;

    public IEnumerator<T> GetEnumerator() { yield return _value; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override int GetHashCode() => _value!.GetHashCode();
}
