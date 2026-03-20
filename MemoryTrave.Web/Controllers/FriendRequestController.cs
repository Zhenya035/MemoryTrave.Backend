using MemoryTrave.Application.Dto;
using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Domain.Common;
using MemoryTrave.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers;

[Route("friends/requests")]
[Authorize]
public class FriendRequestController(
    IFriendRequestService service,
    IWebHostEnvironment env) : BaseController(env)
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] DirectionEnum direction)
    {
        var userId = GetCurrentUserId();

        var result = direction switch
        {
            DirectionEnum.Incoming => await service.GetAllToUserId(userId),
            DirectionEnum.Outgoing => await service.GetAllFromUserId(userId),
            _ => new Result<List<GetFriendDto>>(false, null, "Invalid query", ErrorCode.InvalidInput)
        };

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