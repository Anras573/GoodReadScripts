namespace GoodReadScripts.Utils;

public record Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    private T Value { get; }
    private Error? Error { get; }

    private Result(bool isSuccess, T value, Error? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);

    public static Result<T> Failure(Error error) => new(false, default!, error);

    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure)
        => IsSuccess ? onSuccess(Value) : onFailure(Error!);

    public void Switch(Action<T> onSuccess, Action<Error> onFailure)
    {
        if (IsSuccess)
            onSuccess(Value);
        else
            onFailure(Error!);
    }
    
    public Task SwitchAsync(Func<T, Task> onSuccess, Func<Error, Task> onFailure)
        => IsSuccess ? onSuccess(Value) : onFailure(Error!);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
}