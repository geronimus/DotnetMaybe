# `Geronimus.Maybe` Namespace

A little dotnet library that contains utility functions for dealing with values that may or may not be present.

# What's In The Box ?

## `Maybe`

A static class with methods to help you make `IMaybe<T>` objects, which represent values that may or may not be present.

- `Empty<T>()`
- `ForValue<T>( T value )`

You can use `Maybe` to return results where normal completion could result in either either a value or no value.

## `OpResult`

A static class with methods to help you make `IOpResult` objects, which represent the results of operations.

The methods `Error( Exception error )` and `Success()` are intended for operations that return no value, but for which the caller needs an indication of success or failure.

The generic versions, `Error<T>( Exception error )` and `Success<T>( T value )`, are intended for cases where you do expect a return value.

These methods allow you to incorporate exception objects - which can be a useful source of information about error conditions - into a program's flow of control, without needing to `throw` or `catch` them.

---

# Maybe

```c#
public static class Maybe
```

Contains methods to create instances of values that may or may not be present.

## Methods

### Empty\<T\>

```c#
public static IMaybe<T> Empty<T>()
```

Returns an `EmptyMaybe<T>`, which is an `IMaybe<T>` that does not hold a value.

#### Example

```c#
if ( !userDictionary.ContainsKey( userId ) )
    return Maybe.Empty<User>();
```

### ForValue\<T\>

```c#
public static IMaybe<T> ForValue<T>( T value )
```

Returns an `ValuedMaybe<T>` object, which is an `IMaybe<T>` that holds the value you provided.

#### Example

```c#
if ( userDictionary.ContainsKey( userId ) )
    return Maybe.ForValue( myInnerDictionary[ userId ] );
```

#### Remarks

If you pass in a `null` reference, this method will throw a `NullArgumentException`.

---

# IMaybe\<T\>

```c#
public interface IMaybe<T> : IEnumerable<T>, IEquatable<IMaybe<T>>
```

The interface for values that may or may not be present.

The concrete implementation classes are `EmptyMaybe<T>` for values that are not present, and `ValuedMaybe<T>` when a value is present.

This interface includes the `IEnumerable<T>` interface, meaning you can iterate using a `foreach` or Linq expression to process the value that an object of this type either does or does not contain.

An object of this type provides the following members to help you determine and work with its state:

## Properties

```c#
public bool IsEmpty { get; }
```

`true` for an `EmptyMaybe<T>` and `false` for a `ValuedMaybe<T>`.

## Methods

### GetValue

```c#
public T GetValue();
```

If this is a `ValuedMaybe<T>`, then this method returns the value that it contains. If this is an `EmptyMaybe<T>`, then the method throws an `InvalidOperationException`.

### GetValueOrElse

```c#
public T GetValueOrElse( T defaultValue );
```

A safer way to get the value, without needing to check on the `IsEmpty` state, or resorting to any clever tricks.

If this is a `ValuedMaybe<T>`, then the return value will be the value that this `IMaybe<T>` contains.

But if this is an `EmptyMaybe<T>`, then the return value will be the default value that you provide.

## Remarks

All `EmptyMaybe<T>` instances are equal, provided they are defined with the same type parameter.

`ValuedMaybe<T>` instances are equal when they are defined with the same type parameter and contain values that are equal.

The implementations of these classes are rather straightforward. So if you need further details, we invite you to inspect the source code.

---

# OpResult

```c#
public static class OpResult
```

Contains methods to create instances of `IOpResult` and `IOpResult<T>` objects.

## Error

```c#
public static IOpResult Error( Exception error )
```

Returns an `IOpResult` object representing an operation with a void return type that failed because of the error you provide.

## Error\<T\>

```c#
public static IOpResult<T> Error<T>( Exception error )
```

Returns an `IOpResult<T>` object representing an operation with a the return type that you provide, which failed because of the error you provide.

## Success

```c#
public static IOpResult Success()
```

Returns an `IOpResult` object representing a successful operation with a void return type.

## Success\<T\>

```c#
public static IOpResult<T> Success<T>( T result )
```

Returns an `IOpResult<T>` object representing a successful operation with a the return type and the result you provide.

## IOpResult

Represents the result of an operation that does not return a value.

Successes should have the concrete type `EmptySuccess`. They contain no information other than that the operation was a success and are useful as acknowledgement or "ACK" signals.

All `EmptySuccess` objects should evaluate as equal.

Failures should have the concrete type `ErrorResult` and contain the instance of the exception returned by the failed operation for further analysis by your program.

`ErrorResult` instances do not evaluate as equal because each is presumed to hold a distinct exception object. 

All `IOpResult` objects have the following members:

### Properties

#### IsSuccess

```c#
public bool IsSuccess { get; }
```

Indicates whether or not the operation succeeded.

If the value is true, then the object returned should be of type `EmptySuccess`.

#### IsFailure

```c#
public bool IsFailure { get; }
```

Indicates whether or not the operation failed.

If the value is true, then the object returned should be of type `ErrorResult` and hold the error returned for further analysis by your program.

#### HasError

```c#
public bool HasError { get; }
```

Indicates whether or not this operation result holds an error instance in its `Error` property.

#### Error

```c#
public Exception Error { get; }
```

If the operation resulted in an error, this property will return that error for further analysis by your program.

Otherwise, accessing this property will result in an `InvalidOperationException`.

## IOpResult\<T\>

```c#
public interface IOpResult<T> : IOpResult, IEnumerable<T>
```

Objects with the interface `IOpResult<T>` represent the results of operations intended to return a value, and their type parameter `T` allows users to specify the type of that value.

Successful results should have the concrete type `ValuedSuccess<T>`.

`ValuedSuccess<T>` instances compare as equal when they enclose result values that also compare as equal.

Failed operations should return an instance of the concrete type `ErrorResult<T>`.

`ErrorResult<T>` instances do not compare as equal because each is presumed to hold a distinct exception.

`IOpResult<T>` implements `IEnumerable<T>`, meaning you can iterate across the single result value that may or may not be present using a `foreach` loop or a Linq query.

Because `IOpResult<T>` implements `IOpResult`, it contains all members from that interface, as well as the following additional members:

### Properties

#### HasResult

```c#
public bool HasResult { get; }
```

Will only be `true` for a `ValuedSuccess<T>`.

For an `ErrorResult<T>`, this will be `false`.

#### Result

```c#
public T Result { get; }
```

For a `ValuedSuccess<T>`, this propertu will contain the result.

For an `ErrorResult<T>`, attempting to access this property will throw the exception contained in the `Error` property.

Remember that because this interface already contains all of the members of the `IOpResult` interface, you can check whether the instance your dealing with is a success or an error using the `IsSuccess` and `IsFailure` properties.

### Methods

#### GetResultOrElse

```c#
public T GetResultOrElse( T defaultValue )
```

If the object is a `ValuedSuccess<T>`, then this method will return the value that it contains.

But if it is an `ErrorResult<T>`, then the method will return the default value that you provide.

## Remarks

If you've noticed some resemblance to the Scala class `scala.util.Try<T>`, you would not be wrong. That is the inspiration. However, unlike a `Try`, the `OpResult` object doesn't execute any function and wrap the result in a `Success` or `Failure` object.

Instead, `OpResult` simply helps you to constructs `Success` or `Failure` style objects and leaves the details of the execution up to you.

If pattern matching is more your cup of tea, then know that an `EmptySuccess` is tagged with the empty interface `ISuccessResult`, and a `ValuedSuccess<T>` is tagged with `ISuccessResult` and `IValueHolder`.

`ErrorResult` and `ErrorResult<T>` are both tagged with `IErrorResult`.

The implementation of the concrete classes is rather straightforward. If you're interested in further details, we encourage you to look into the source code.

---

# Why Maybe?

We've worked a lot with Scala and found that it has some very useful constructs for handling values with a _zero-or-one_ multiplicity, as well as successes and failures.

These humble classes aren't anything like a "port" of the Scala originals that inspired them. (Which, if you're curious, are `Option` and `Try`.)

Instead, we're just interested in providing some useful behaviour that avoids the need to deal with `null`, or with any default sentinel value. (`IMaybe`)

And we note that [Microsoft recommends against throwing exceptions, unless something is very wrong](https://docs.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions#handle-common-conditions-without-throwing-exceptions). And yet, exceptions can be a very effective mechanism for understanding what is going wrong in a system. The classes that implement `IOpResult` provide a way of trading in exceptions, without constantly throwing and catching them. (Since it's building the stack trace that would seem to be the primary performance concern.)

And yes, you would be right to notice that the idea is also very much like Go's return types. There is no new thing under the sun.

And while we're settling our debts, Scala certainly doesn't call any of these concepts by the name `Maybe`. But Haskell certainly does.

We won't pretend that our implementation bears any more than a vague resemblance to the pure monadic ideal. This little library is to the _Maybe Monad_ what a clip-on bow-tie is to a Savile Row dinner suit.
