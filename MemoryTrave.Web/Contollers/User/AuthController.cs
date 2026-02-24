using FluentValidation;
using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Application.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Contollers.User;

[ApiController]
[Route("users/auth")]
public class AuthController(
    IValidator<RegistrationDto> regValidator,
    IValidator<AuthorizationDto> authValidator,
    IValidator<AddKeysDto> addKeysValidator,
    IRegistrationUseCase regUseCase,
    IAuthorizationUseCase authUseCase) : ControllerBase
{
    [HttpGet("keys/private")]
    [Authorize]
    public async Task<ActionResult<PrivateKeyResponseDto>> GetPrivateKey() =>
        Ok(await authUseCase.GetPrivateKey());
    
    [HttpPost("registration")]
    public async Task<ActionResult<AuthorizationResponseDto>> Registration([FromBody] RegistrationDto reg)
    {
        var validResult = await regValidator.ValidateAsync(reg);
        if(!validResult.IsValid)
            return BadRequest(validResult.Errors);
        
        return Ok(await regUseCase.Registration(reg));
    }

    [HttpPost("authorization")]
    public async Task<ActionResult<AuthorizationResponseDto>> Authorization([FromBody] AuthorizationDto auth)
    {
        var validResult = await authValidator.ValidateAsync(auth);
        if(!validResult.IsValid)
            return BadRequest(validResult.Errors);
        
        return Ok(await authUseCase.Authorization(auth));
    }

    [HttpPut("add/keys")]
    [Authorize]
    public async Task<IActionResult> AddKeys([FromBody] AddKeysDto addKeys)
    {
        var validResult = await addKeysValidator.ValidateAsync(addKeys);
        if(!validResult.IsValid)
            return BadRequest(validResult.Errors);

        await regUseCase.AddKeys(addKeys);
        
        return NoContent();
    }
}