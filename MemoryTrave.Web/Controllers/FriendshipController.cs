using MemoryTrave.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers;

[Route("friends")]
[Authorize]
public class FriendshipController(IWebHostEnvironment env, IFriendshipService service) : BaseController(env)
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        
        var result = await service.GetAll(userId);
        return HandleResult(result);
    }

    [HttpGet("keys")]
    public async Task<IActionResult> GetFriendsKeys()
    {
        var userId = GetCurrentUserId();
        
        var result = await service.GetPublicKeys(userId);
        return HandleResult(result);
    }

    [HttpDelete("{friendshipId:guid}")]
    public async Task<IActionResult> Delete(Guid friendshipId)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.Delete(userId, friendshipId);
        return HandleResult(result);
    }
}