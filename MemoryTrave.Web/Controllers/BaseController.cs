using System.Security.Claims;
using FluentValidation.Results;
using MemoryTrave.Application.Dto;
using MemoryTrave.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Success<T>(T data, string? message = null) =>
        base.Ok(new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        });

    protected IActionResult Success() =>
        base.Ok(new ApiResponse { Success = true });

    protected IActionResult IsCreated<T>(T data) =>
        StatusCode(201, new ApiResponse<T>
        {
            Success =  true,
            Data = data
        });

    protected IActionResult ValidFailed(ValidationResult result)
    {
        var formattedErrors = result.Errors.Select(e => e.ErrorMessage).ToList();
        
        return base.BadRequest(new ApiResponse
        {
            Success = false,
            Message = "Validation failed",
            Errors = formattedErrors
        });
    }
    
    protected IActionResult ValidFailed(string error)
    {
        return base.BadRequest(new ApiResponse
        {
            Success = false,
            Message = "Validation failed",
            Errors = error
        });
    }

    protected Guid GetCurrentUserId()
    {
        var claim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null || !Guid.TryParse(claim.Value, out var userId))
            throw new UnAuthorizedException("token");
        
        return userId;
    }
}