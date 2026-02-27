using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.User;

[Route("users/auth")]
public class AuthController(IUserService service) : BaseController
{
    [HttpGet("keys/private")]
    [Authorize]
    public async Task<IActionResult> GetPrivateKey()
    {
        var userId = GetCurrentUserId();
        
        var result = await service.GetPrivateKey(userId);
        
        return HandleResult(result);
    }
    
    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] RegistrationDto reg)
    {
        var result = await service.Registration(reg);
        return HandleResult(result);
    }

    [HttpPost("authorization")]
    public async Task<IActionResult> Authorization([FromBody] AuthorizationDto auth)
    {
        var result = await service.Authorization(auth);
        return HandleResult(result);
    }

    [HttpPut("add/keys")]
    [Authorize]
    public async Task<IActionResult> AddKeys([FromBody] AddKeysDto addKeys)
    {
        var userId = GetCurrentUserId();
        
        var result = await service.AddKeys(addKeys, userId);
        
        return HandleResult(result);
    }
}