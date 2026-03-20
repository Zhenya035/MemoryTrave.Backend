namespace MemoryTrave.Domain.Exceptions;

public class UnauthorizedAccessException() : Exception($"Invalid or missing user token");