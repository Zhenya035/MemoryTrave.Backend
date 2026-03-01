namespace MemoryTrave.Domain.Common;

public enum ErrorCode
{
    InvalidInput = 400,
    Unauthorized = 401,
    AccessDenied = 403,
    NotFound = 404,
    AlreadyExists = 409
}