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
        return Success();
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm()
    {
        return Success();
    }

    [HttpDelete("cancel")]
    public async Task<IActionResult> Cancel()
    {
        return Success();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        return Success();
    }
}