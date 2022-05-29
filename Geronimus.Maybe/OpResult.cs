using System.Collections;

namespace Geronimus.Maybe;

public interface IErrorResult : IEmptyValue {}

public interface IOpResult
{
    public Exception Error { get; }
    public bool HasError { get; }
    public bool IsFailure { get; }
    public bool IsSuccess { get; }
}

public interface IOpResult<T> : IOpResult, IEnumerable<T>
{
    public bool HasResult { get; }
    public T Result { get; }

    public T GetResultOrElse( T defaultValue );
}

public interface ISuccessResult {}

public static class OpResult
{
    public static IOpResult Error( Exception error ) =>
        new ErrorResult( error );

    public static IOpResult<T> Error<T>( Exception error ) =>
        new ErrorResult<T>( error );

    public static IOpResult Success() => new EmptySuccess();

    public static IOpResult<T> Success<T>( T result ) =>
        new ValuedSuccess<T>( result );
}

public sealed class EmptySuccess : IOpResult, ISuccessResult, IEmptyValue
{
    // Constructor:
    public EmptySuccess() {}

    // Properties:
    public Exception Error =>
        throw new InvalidOperationException(
            $"A { nameof( ISuccessResult ) } does not contain a value for " +
                $"the property { nameof( Error ) }."
        );
    
    public bool HasError => false;
    public bool HasResult => false;
    public bool IsFailure => false;
    public bool IsSuccess => true;

    public override bool Equals( object? obj ) =>
        obj != null && obj is EmptySuccess;

    public override int GetHashCode() => 0;
}

public class ErrorResult : IOpResult, IErrorResult
{
    // Fields:
    protected Exception _error;
    protected bool _usesDefaultError = false;

    // Constructor:
    public ErrorResult( Exception error )
    {
        if ( error == null )
        {
            _error = new ErrorNotProvidedException();
            _usesDefaultError = true;
        }
        else
        {
            _error = error;
        }
    }

    // Properties:
    public Exception Error => _error;
    public bool HasError => !_usesDefaultError;
    public bool IsFailure => true;
    public bool IsSuccess => false;
}

public sealed class ErrorResult<T> : ErrorResult, IOpResult<T>
{
    // Constructor: 
    public ErrorResult( Exception error ) : base( error ) {}

    // Properties:
    public bool HasResult => false;
    public T Result { get { throw _error; } }

    // Methods:
    public IEnumerator<T> GetEnumerator() { yield break; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public T GetResultOrElse( T defaultValue ) => defaultValue;
}

public sealed class ValuedSuccess<T> :
    IOpResult<T>,
    ISuccessResult,
    IValueHolder
{
    // Fields:
    private T _result;

    // Constructor:
    public ValuedSuccess( T result )
    {
        if ( result == null )
            throw new ArgumentNullException( nameof( result ) );
        else
            _result = result;
    }

    // Properties:
    public Exception Error =>
        throw new InvalidOperationException(
            $"A { nameof( ISuccessResult ) } does not contain a value for " +
                $"the property { nameof( Error ) }."
        );
    
    
    public bool HasError => false;
    public bool HasResult => true;
    public bool IsFailure => false;
    public bool IsSuccess => true;
    public T Result => _result;

    // Methods:
    public override bool Equals( object? obj ) =>
        obj != null &&
            obj is ValuedSuccess<T> that &&
            _result!.Equals( that.Result );

    public IEnumerator<T> GetEnumerator() { yield return _result; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public T GetResultOrElse( T defaultValue ) => _result;

    public override int GetHashCode() => _result!.GetHashCode();
}
