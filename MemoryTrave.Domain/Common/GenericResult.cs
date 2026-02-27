namespace MemoryTrave.Domain.Common;

public class Result<T>(bool isSuccess, T? data, string error, ErrorCode? errorCode) 
    : Result(isSuccess, error, errorCode)
{
    public T? Data { get; init; } = data;

    public static Result<T> Success(T data) =>
        new(true, data, string.Empty,  null);

    public new static Result<T> Failure(string error, ErrorCode? errorCode) =>
        new(false, default, error, errorCode);
}