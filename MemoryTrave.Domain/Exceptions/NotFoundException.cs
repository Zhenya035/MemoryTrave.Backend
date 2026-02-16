namespace MemoryTrave.Domain.Exceptions;

public class NotFoundException(string message) : Exception($"{message} not found");