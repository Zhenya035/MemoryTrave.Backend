using MemoryTrave.Application.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.User;

[ApiController]
[Route("users/profile")]
[Authorize]
public class ProfileController(IUserService service) : BaseController
{
    [HttpGet("my")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = GetCurrentUserId();
        var result = await service.GetProfile(userId);
        return HandleResult(result);
    }

    [HttpGet("blocks")]
    public async Task<IActionResult> GetBlockUsers()
    {
        var userId = GetCurrentUserId();
        
        var result = await service.GetBlockUsers(userId);
        return HandleResult(result);
    }
}