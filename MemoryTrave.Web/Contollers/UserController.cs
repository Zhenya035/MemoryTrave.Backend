using FluentValidation;
using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Application.Services;
using MemoryTrave.Application.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Contollers;

[ApiController]
[Route("users")]
public class UserController(
    IValidator<RegistrationDto> regValidator,
    IValidator<AuthorizationDto> authValidator,
    IValidator<AddKeysDto> addKeysValidator,
    IUserService service,
    IRegistrationUseCase regUseCase,
    IAuthorizationUseCase authUseCase) : ControllerBase
{
    [HttpGet("private-key")]
    [Authorize]
    public async Task<ActionResult<PrivateKeyResponceDto>> GetPrivateKey() =>
        Ok(await service.GetPrivateKey());
    
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

    [HttpPost("add-keys")]
    [Authorize]
    public async Task<IActionResult> AddKeys([FromBody] AddKeysDto addKeys)
    {
        var validResult = await addKeysValidator.ValidateAsync(addKeys);
        if(!validResult.IsValid)
            return BadRequest(validResult.Errors);

        await service.AddKeys(addKeys);
        
        return NoContent();
    }
}