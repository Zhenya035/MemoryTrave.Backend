using FluentValidation;
using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using Microsoft.AspNetCore.Mvc;

namespace MemoryTrave.Web.Contollers;

[ApiController]
[Route("users")]
public class UserController(IValidator<RegistrationDto> _validator) : ControllerBase
{
    [HttpPost("registration")]
    public async Task<ActionResult<AuthorizationResponseDto>> Registration([FromBody] RegistrationDto user)
    {
        var result = await _validator.ValidateAsync(user);
        
        if(!result.IsValid)
            BadRequest(result.Errors);
        
        return Ok(result);
    }
}