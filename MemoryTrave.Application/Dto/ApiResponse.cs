namespace MemoryTrave.Application.Dto;

public class ApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Errors { get; set; }
}

public class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }
}