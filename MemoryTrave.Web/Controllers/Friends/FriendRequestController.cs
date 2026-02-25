using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Friends;

[Route("friends/requests")]
[Authorize]
public class FriendRequestController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm()
    {
        return Ok();
    }

    [HttpDelete("cancel")]
    public async Task<IActionResult> Cancel()
    {
        return Ok();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        return Ok();
    }
}