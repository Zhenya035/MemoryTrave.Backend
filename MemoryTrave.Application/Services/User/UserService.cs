using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Application.Interfaces.Jwt;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Application.Mapping;
using MemoryTrave.Domain.Exceptions;
using MemoryTrave.Domain.Interfaces;

namespace MemoryTrave.Application.Services.User;

public class UserService(
    IUserRepository userRepository,
    IFriendshipRepository friendRepository,
    IJwtService jwtService) : IUserService
{
    public async Task<AuthorizationResponseDto> Authorization(AuthorizationDto authUser)
    {
        var user = await userRepository.GetByEmailForAuth(authUser.Email);
        
        if (user == null)
            throw new UnAuthorizedException("User");
        
        if(user.IsBlocked)
            throw new UserBannedException();
        
        if(!BCrypt.Net.BCrypt.Verify(authUser.Password, user.PasswordHash))
            throw new UnAuthorizedException("Password");
        
        var token = jwtService.GenerateJwt(user);

        var response = new AuthorizationResponseDto()
        {
            JwtToken = token,
        };
        
        return response;
    }
    
    public async Task<PrivateKeyResponseDto> GetPrivateKey(Guid userId)
    {
        if (!await userRepository.UserExistsById(userId))
            throw new NotFoundException("User");
        
        var encryptedPrivateKey = await userRepository.GetKeyById(userId);
        
        if(encryptedPrivateKey == null)
            throw new NotFoundException("PrivateKey");

        var response = new PrivateKeyResponseDto()
        {
            EncryptedPrivateKey = encryptedPrivateKey
        };
        return response;
    }

    public async Task<AuthorizationResponseDto> Registration(RegistrationDto regUser)
    {
        if (await userRepository.UserExistsByEmail(regUser.Email))
            throw new AlreadyAddedException("This email");
        
        var user = UserMapping.MapFromRegistrationDto(regUser);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(regUser.Password);
        user.Id = Guid.NewGuid();

        user = await userRepository.Registration(user);
        
        var token = jwtService.GenerateJwt(user);

        var response = new AuthorizationResponseDto()
        {
            JwtToken = token,
        };
        
        return response;
    }
    
    public async Task AddKeys(AddKeysDto keys, Guid userId)
    {
        if (!await userRepository.UserExistsById(userId))
            throw new NotFoundException("User");
        
        await userRepository.AddKey(userId, keys.PublicKey, keys.EncryptedPrivateKey);
    }
    
    public async Task<GetProfileDto> GetProfile(Guid userId)
    {
        var user = await userRepository.GetByIdWithArticles(userId);
        if (user == null)
            throw new NotFoundException("User");
        
        var response = UserMapping.MapToGetProfileDto(user);

        var friends = await friendRepository.GetAllFriends(user.Id);
        response.FriendsCount = friends.Count;
        
        return response;
    }

    public async Task<List<GetUserDto>> GetBlockUsers(Guid userId)
    {
        var user = await userRepository.GetUserById(userId);
        if (user == null)
            throw new NotFoundException("User");

        if (user.BlockedUsers.Count == 0)
            return [];
        
        var blockUsers = await userRepository.GetBlockUsers(user.BlockedUsers);

        return blockUsers.Select(UserMapping.MapToGetUserDto).ToList();
    }

    public Task DeleteProfile(Guid userId)
    {
        throw new NotImplementedException();
    }
}