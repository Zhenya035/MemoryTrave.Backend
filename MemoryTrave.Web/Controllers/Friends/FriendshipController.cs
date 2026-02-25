using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Friends;

[Route("friends")]
[Authorize]
public class FriendshipController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }

    [HttpDelete("delete/{friendId}")]
    public async Task<IActionResult> Delete(Guid friendId)
    {
        return Ok();
    }
}