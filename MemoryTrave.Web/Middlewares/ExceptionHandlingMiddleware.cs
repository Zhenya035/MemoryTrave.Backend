using System.Text.Json;
using MemoryTrave.Domain.Exceptions;

namespace MemoryTrave.Web.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleException(context, e);
        }
    }

    private static async Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = StatusCodes.Status500InternalServerError;
        var message = "An unexpected error occurred.";
        var detailed = string.Empty;

        switch (exception)
        {
            case AlreadyAddedException alrAddEx:
                statusCode = StatusCodes.Status409Conflict;
                message = "Resource already exists.";
                detailed = alrAddEx.Message;
                break;
            case InvalidInputDataException invInpEx:
                statusCode = StatusCodes.Status400BadRequest;
                message = "Invalid input.";
                detailed = invInpEx.Message;
                break;
            case IncorrectAuthorizationException invAuthEx:
                statusCode = StatusCodes.Status401Unauthorized;
                message = "Authorization failed.";
                detailed = invAuthEx.Message;
                break;
            case NotFoundException nFEx:
                statusCode = StatusCodes.Status404NotFound;
                message = "Resource not found.";
                detailed = nFEx.Message;
                break;
            default:
                detailed = exception.Message;
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = new
        {
            StatusCode = statusCode,
            Message = message,
            Detailed = detailed
        };
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}