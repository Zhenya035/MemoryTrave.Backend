namespace MemoryTrave.Domain.Common;

public class Result(bool isSuccess, string error, ErrorCode? errorCode)
{
    public bool IsSuccess { get; init; } = isSuccess;
    public string? Error  { get; init; } = error;
    public ErrorCode? ErrorCode { get; init; } = errorCode;

    public static Result Success() =>
        new(true, string.Empty, null);

    public static Result Failure(string error, ErrorCode? errorCode) =>
        new(false, error, errorCode);
}