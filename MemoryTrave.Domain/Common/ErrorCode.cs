namespace MemoryTrave.Domain.Common;

public enum ErrorCode
{
    InvalidInput = 400,
    UserBanned = 401,
    Unauthorized = 403,
    NotFound = 404,
    AlreadyExists = 409
}