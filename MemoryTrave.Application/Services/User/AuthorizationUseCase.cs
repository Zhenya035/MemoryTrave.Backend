using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Domain.Exceptions;
using MemoryTrave.Domain.Interfaces;

namespace MemoryTrave.Application.Services.User;

public class AuthorizationUseCase(IUserRepository userRepository, IJwtService jwtService) : IAuthorizationUseCase
{
    public async Task<AuthorizationResponseDto> Authorization(AuthorizationDto authUser)
    {
        var user = await userRepository.GetByEmail(authUser.Email);
        
        if (user == null)
            throw new NotFoundException("User");
        
        if(user.IsBlocked)
            throw new UserBannedException(user.Email);
        
        if(!BCrypt.Net.BCrypt.Verify(authUser.Password, user.PasswordHash))
            throw new InvalidInputDataException("Password is incorrect");
        
        var token = jwtService.GenerateJwt(user);

        var response = new AuthorizationResponseDto()
        {
            JwtToken = token,
        };
        
        return response;
    }
}