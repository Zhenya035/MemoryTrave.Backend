using MemoryTrave.Application.Interfaces.User;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.User;

[Route("users")]
public class UserController(IUserService service) : BaseController
{
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var userId = GetCurrentUserId();
        
        await service.Delete(userId);
        return Success();
    }
}