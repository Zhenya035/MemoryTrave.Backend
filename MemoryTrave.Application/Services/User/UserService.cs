using AutoMapper;
using FluentValidation;
using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Application.Interfaces.Jwt;
using MemoryTrave.Application.Interfaces.User;
using MemoryTrave.Domain.Common;
using MemoryTrave.Domain.Interfaces;

namespace MemoryTrave.Application.Services.User;

public class UserService(
    IUserRepository userRepository,
    IFriendshipRepository friendRepository,
    IJwtService jwtService,
    IMapper mapper,
    IValidationService validationService) : IUserService
{
    public async Task<Result<AuthorizationResponseDto>> Authorization(AuthorizationDto authUser)
    {
        var validResult = await validationService.Validate(authUser);
        if (!validResult.IsSuccess)
            return Result<AuthorizationResponseDto>.Failure(validResult.Error, validResult.ErrorCode);
        
        var user = await userRepository.GetByEmailForAuth(authUser.Email);

        if (user == null)
            return Result<AuthorizationResponseDto>.Failure($"User with email: {authUser.Email} not found",
                ErrorCode.NotFound);
        
        if(user.IsBlocked)
            return Result<AuthorizationResponseDto>.Failure($"User is blocked",
                ErrorCode.UserBanned);
        
        if(!BCrypt.Net.BCrypt.Verify(authUser.Password, user.PasswordHash))
            return Result<AuthorizationResponseDto>.Failure($"Invalid password",
                ErrorCode.Unauthorized);
        
        var token = jwtService.GenerateJwt(user);

        var resultDto = new AuthorizationResponseDto()
        {
            JwtToken = token,
        };
        var response = Result<AuthorizationResponseDto>.Success(resultDto);
        
        return response;
    }
    
    public async Task<Result<PrivateKeyResponseDto>> GetPrivateKey(Guid userId)
    {
        var isExist = await userRepository.UserExistsById(userId);
        if (!isExist)
            return Result<PrivateKeyResponseDto>.Failure($"User not found",
                ErrorCode.NotFound);
        
        var encryptedPrivateKey = await userRepository.GetKeyById(userId);
        
        if(encryptedPrivateKey == null)
            return Result<PrivateKeyResponseDto>.Failure($"Private key not found",
                ErrorCode.NotFound);

        var resultDto = new PrivateKeyResponseDto()
        {
            EncryptedPrivateKey = encryptedPrivateKey
        };
        var response = Result<PrivateKeyResponseDto>.Success(resultDto);
        
        return response;
    }

    public async Task<Result<AuthorizationResponseDto>> Registration(RegistrationDto regUser)
    {
        var validResult = await validationService.Validate(regUser);
        if (!validResult.IsSuccess)
            return Result<AuthorizationResponseDto>.Failure(validResult.Error, validResult.ErrorCode);
        
        var isExist = await userRepository.UserExistsByEmail(regUser.Email);
        if (isExist)
            return Result<AuthorizationResponseDto>.Failure("Email already exists", ErrorCode.AlreadyExists);
        
        var user =  mapper.Map<Domain.Models.User>(regUser);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(regUser.Password);
        user.Id = Guid.NewGuid();

        user = await userRepository.Registration(user);
        
        var token = jwtService.GenerateJwt(user);

        var resultDto = new AuthorizationResponseDto()
        {
            JwtToken = token,
        };
        var response = Result<AuthorizationResponseDto>.Success(resultDto);
        
        return response;
    }
    
    public async Task<Result> AddKeys(AddKeysDto keys, Guid userId)
    {
        var validResult = await validationService.Validate(keys);
        if (!validResult.IsSuccess)
           return Result.Failure(validResult.Error, validResult.ErrorCode);
        
        var isExist = await userRepository.UserExistsById(userId);
        if (!isExist)
            return Result.Failure("User not found", ErrorCode.NotFound);
        
        await userRepository.AddKey(userId, keys.PublicKey, keys.EncryptedPrivateKey);
        return Result.Success();
    }
    
    public async Task<Result<GetProfileDto>> GetProfile(Guid userId)
    {
        var user = await userRepository.GetByIdWithArticles(userId);
        if (user == null)
            return Result<GetProfileDto>.Failure("User not found", ErrorCode.NotFound);
        
        var resultDto = mapper.Map<GetProfileDto>(user);

        var friends = await friendRepository.GetAllFriends(user.Id);
        resultDto.FriendsCount = friends.Count;
        
        var response = Result<GetProfileDto>.Success(resultDto);
        
        return response;
    }

    public async Task<Result<List<GetUserDto>>> GetBlockUsers(Guid userId)
    {
        var user = await userRepository.GetUserById(userId);
        if (user == null)
            return Result<List<GetUserDto>>.Failure("User not found", ErrorCode.NotFound);
        
        var blockUsers = await userRepository.GetBlockUsers(user.BlockedUsers);
        var resultDto = mapper.Map<List<GetUserDto>>(blockUsers);
        
        var response = Result<List<GetUserDto>>.Success(resultDto);

        return response;
    }

    public async Task<Result> Delete(Guid userId)
    {
        var isExists = await userRepository.UserExistsById(userId);
        if (!isExists)
            return Result.Failure("User not found", ErrorCode.NotFound);
        
        await userRepository.Delete(userId);
        return Result.Success();
    }
}