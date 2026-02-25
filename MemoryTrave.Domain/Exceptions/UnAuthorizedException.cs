namespace MemoryTrave.Domain.Exceptions;

public class UnAuthorizedException(string message) : Exception($"Invalid {message}");