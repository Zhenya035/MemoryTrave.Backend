using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Application.Interfaces.Profile;
using MemoryTrave.Application.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Contollers.User;

[ApiController]
[Route("users/profile")]
[Authorize]
public class ProfileController(IProfileService service) : ControllerBase
{
    [HttpGet("my")]
    public async Task<ActionResult<GetProfileDto>> GetMyProfile() =>
        Ok(await service.GetProfile());

    [HttpGet("blocks")]
    public async Task<ActionResult<List<GetUserDto>>> GetBlockUsers() =>
        Ok(await service.GetBlockUsers());
}