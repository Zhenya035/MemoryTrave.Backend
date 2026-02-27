using System.Security.Claims;
using MemoryTrave.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers;

[ApiController]
public abstract class BaseController() : ControllerBase
{
   protected IActionResult HandleResult<T>(Result<T> result)
   {
      if (result.IsSuccess)
         return Ok(result.Data);
      
      return StatusCode((int)result.ErrorCode!, new ProblemDetails
      {
         Title = GetTitleForStatus((int)result.ErrorCode),
         Detail = result.Error,
         Instance = HttpContext.Request.Path
      });
   }
   
   protected IActionResult HandleResult(Result result)
   {
      if (result.IsSuccess)
         return NoContent();
      
      return StatusCode((int)result.ErrorCode, new ProblemDetails
      {
         Status = (int)result.ErrorCode,
         Title = GetTitleForStatus((int)result.ErrorCode),
         Detail = result.Error,
         Instance = HttpContext.Request.Path
      });
   }

   protected Guid GetCurrentUserId()
   {
      var claim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
      if (claim == null || !Guid.TryParse(claim.Value, out var userId))
         throw new UnauthorizedAccessException(); 
      
      return userId;
   }
   
   private string GetTitleForStatus(int errorCode) => errorCode switch
   {
      400 => "Bad Request",
      401 => "Unauthorized",
      403 => "Forbidden",
      404 => "Not Found",
      409 => "Conflict",
      _ => "Server Error"
   };
}