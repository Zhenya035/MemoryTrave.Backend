using MemoryTrave.Application.Dto;
using MemoryTrave.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.User;

[Route("users")]
[Authorize]
public class UserController(IUserService service, IWebHostEnvironment env) : BaseController(env)
{
    [HttpGet("available")]
    public async Task<IActionResult> GetAllWithoutMe()
    {
        var userId = GetCurrentUserId();
        
        var result = await service.GetUsersWithoutMe(userId);
        return HandleResult(result);
    }

    [HttpPut("block")]
    public async Task<IActionResult> Block(ListIdDto userIds)
    {
        var userId = GetCurrentUserId();

        var result = await service.Block(userIds, userId);
        return HandleResult(result);
    }

    [HttpPut("unblock")]
    public async Task<IActionResult> Unblock(ListIdDto userIds)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.Unblock(userIds, userId);
        return HandleResult(result);
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var userId = GetCurrentUserId();
        
        var result = await service.Delete(userId);
        return HandleResult(result);
    }
}