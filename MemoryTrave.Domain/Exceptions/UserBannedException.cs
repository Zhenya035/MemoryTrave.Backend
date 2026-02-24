namespace MemoryTrave.Domain.Exceptions;

public class UserBannedException(string message) : Exception($"User with email: {message} is banned");