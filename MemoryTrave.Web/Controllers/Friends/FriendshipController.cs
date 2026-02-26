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
        return Success();
    }

    [HttpDelete("{friendId:guid}")]
    public async Task<IActionResult> Delete(Guid friendId)
    {
        return Success();
    }
}