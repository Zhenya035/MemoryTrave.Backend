using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Application.Mapping;
using MemoryTrave.Domain.Exceptions;
using MemoryTrave.Domain.Interfaces;

namespace MemoryTrave.Application.Services.User;

public class RegistrationUseCase(
    IUserRepository userRepository,
    IJwtService jwtService) : IRegistrationUseCase
{
    public async Task<AuthorizationResponseDto> Registration(RegistrationDto regUser)
    {
        if (await userRepository.UserExists(regUser.Email))
            throw new AlreadyAddedException("This email");
        
        var user = UserMapping.MapFromRegistrationDto(regUser);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(regUser.Password);

        user = await userRepository.Registration(user);
        
        var token = jwtService.GenerateJwt(user);

        var response = new AuthorizationResponseDto()
        {
            JwtToken = token,
        };
        
        return response;
    }
}