using FluentValidation;
using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Application.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Controllers.User;

[Route("users/auth")]
public class AuthController(
    IValidator<RegistrationDto> regValidator,
    IValidator<AuthorizationDto> authValidator,
    IValidator<AddKeysDto> addKeysValidator,
    IUserService service) : BaseController
{
    [HttpGet("keys/private")]
    [Authorize]
    public async Task<IActionResult> GetPrivateKey()
    {
        var userId = GetCurrentUserId();
        
        var keys = await service.GetPrivateKey(userId);
        
        return Ok(keys);
    }
    
    [HttpPost("registration")]
    public async Task<IActionResult> Registration([FromBody] RegistrationDto reg)
    {
        var validResult = await regValidator.ValidateAsync(reg);
        if(!validResult.IsValid)
            return BadRequest(validResult);

        var token = await service.Registration(reg);
        return Created(token);
    }

    [HttpPost("authorization")]
    public async Task<IActionResult> Authorization([FromBody] AuthorizationDto auth)
    {
        var validResult = await authValidator.ValidateAsync(auth);
        if(!validResult.IsValid)
            return BadRequest(validResult.Errors);

        var token = await service.Authorization(auth);
        return Ok(token);
    }

    [HttpPut("add/keys")]
    [Authorize]
    public async Task<IActionResult> AddKeys([FromBody] AddKeysDto addKeys)
    {
        var validResult = await addKeysValidator.ValidateAsync(addKeys);
        if(!validResult.IsValid)
            return BadRequest(validResult.Errors);

        var userId = GetCurrentUserId();
        
        await service.AddKeys(addKeys, userId);
        
        return Ok();
    }
}