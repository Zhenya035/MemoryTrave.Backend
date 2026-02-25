using System.Text.Json;
using MemoryTrave.Application.Dto;
using MemoryTrave.Domain.Exceptions;

namespace MemoryTrave.Web.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
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

    private async Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var statusCode = StatusCodes.Status500InternalServerError;
        string message;

        switch (exception)
        {
            case AlreadyAddedException alrAddEx:
                statusCode = StatusCodes.Status409Conflict;
                message = env.IsDevelopment() ? alrAddEx.Message : "Resource already exists.";
                break;
            case InvalidInputDataException invInpEx:
                statusCode = StatusCodes.Status400BadRequest;
                message = env.IsDevelopment() ? invInpEx.Message : "Invalid input data.";
                break;
            case NotFoundException nFEx:
                statusCode = StatusCodes.Status404NotFound;
                message = env.IsDevelopment() ? nFEx.Message : "Resource not found.";
                break;
            case UserBannedException uBE:
                statusCode = StatusCodes.Status403Forbidden;
                message = env.IsDevelopment() ? uBE.Message : "Access denied.";
                break;
            case UnAuthorizedException unAuthEx:
                statusCode = StatusCodes.Status401Unauthorized;
                message = env.IsDevelopment() ? unAuthEx.Message : "Invalid credentials or token.";
                break;
            default:
                message = env.IsDevelopment() ? exception.Message : "An unexpected error occurred.";
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = new ApiResponse()
        {
            Success =  false,
            Message = message,
        };
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}