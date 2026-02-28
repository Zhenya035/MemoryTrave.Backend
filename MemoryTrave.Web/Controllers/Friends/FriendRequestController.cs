using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Friends;

[Route("friends/requests")]
[Authorize]
public class FriendRequestController(IWebHostEnvironment env) : BaseController(env)
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        throw new Exception();
    }

    [HttpPost("confirm")]
    public async Task<IActionResult> Confirm()
    {
        throw new Exception();
    }

    [HttpDelete("cancel")]
    public async Task<IActionResult> Cancel()
    {
        throw new Exception();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        throw new Exception();
    }
}