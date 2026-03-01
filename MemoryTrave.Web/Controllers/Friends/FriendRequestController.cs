using MemoryTrave.Application.Dto.Requests;
using MemoryTrave.Application.Interfaces.Friend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Friends;

[Route("friends/requests")]
[Authorize]
public class FriendRequestController(
    IFriendRequestService service,
    IWebHostEnvironment env) : BaseController(env)
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        
        var result = await service.GetAllByUserId(userId);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IdDto dto)
    {
        var userId = GetCurrentUserId();

        var result = await service.Create(userId, dto.Id);
        return HandleResult(result);
    }

    [HttpPost("{requestId:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid requestId)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.Confirm(userId, requestId);
        return HandleResult(result);
    }

    [HttpDelete("{requestId:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid requestId)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.Cancel(userId, requestId);
        return HandleResult(result);
    }
}