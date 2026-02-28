using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.Friends;

[Route("friends")]
[Authorize]
public class FriendshipController(IWebHostEnvironment env) : BaseController(env)
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        throw new Exception();
    }

    [HttpDelete("{friendId:guid}")]
    public async Task<IActionResult> Delete(Guid friendId)
    {
        throw new Exception();
    }
}