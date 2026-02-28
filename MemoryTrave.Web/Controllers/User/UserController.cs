using MemoryTrave.Application.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.User;

[Route("users")]
public class UserController(IUserService service, IWebHostEnvironment env) : BaseController(env)
{
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var userId = GetCurrentUserId();
        
        var result = await service.Delete(userId);
        return HandleResult(result);
    }
}