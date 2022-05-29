namespace Geronimus.Maybe;

public class ErrorNotProvidedException : Exception
{
    private const string _defaultMessage =
        "No explanation was provided for the failure.";
    private Exception? _passedInner = null;

    public ErrorNotProvidedException() : base( _defaultMessage ) {}

    public ErrorNotProvidedException( string message = _defaultMessage ) :
        base( message ) {}

    public ErrorNotProvidedException(
        string message,
        Exception inner
    ) : base(
        string.IsNullOrEmpty( message ) ? _defaultMessage : message,
        inner
    ) {
        _passedInner = inner;
    }

    // All instances of this exception are logically equivalent, except in cases
    // where they contain different inner exceptions. (None should, since this
    // is precisely the exception to throw when the appropriate exception type
    // is not found.)
    public override bool Equals( object? obj ) =>
        obj != null &&
            obj is ErrorNotProvidedException that &&
            (
                _passedInner == null ||
                _passedInner.Equals( that._passedInner )
            );

    public override int GetHashCode() =>
        _passedInner != null ? _passedInner.GetHashCode() : 1;
}
