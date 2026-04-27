using AutoMapper;
using MemoryTrave.Application.Dto;
using MemoryTrave.Application.Dto.Requests.User;
using MemoryTrave.Application.Dto.Responses;
using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Application.Dto.Responses.User;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Domain.Common;
using MemoryTrave.Domain.Enums;
using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Services;

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
                ErrorCode.Unauthorized);
        
        if(!BCrypt.Net.BCrypt.Verify(authUser.Password, user.PasswordHash))
            return Result<AuthorizationResponseDto>.Failure($"Invalid password",
                ErrorCode.AccessDenied);
        
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
        var isExist = await userRepository.ExistsById(userId);
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
        
        var isExist = await userRepository.ExistsByEmail(regUser.Email);
        if (isExist)
            return Result<AuthorizationResponseDto>.Failure("Email already exists", ErrorCode.AlreadyExists);
        
        var user =  mapper.Map<User>(regUser);
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
        
        var isExist = await userRepository.ExistsById(userId);
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

        var access = user.Articles
            .Where(a => a.Visibility == VisibilityEnum.Private && a.EncryptedKeys != null)
            .SelectMany(a => a.EncryptedKeys.Where(k => k.UserId == userId)
                .Select(k => new { a.Id, k.EncryptedKey}))
            .ToDictionary(x => x.Id, x => x.EncryptedKey);
        
        foreach (var article in resultDto.Articles)
        {
            if (article.IsPrivate && access.TryGetValue(article.Id, out var key))
                article.EncryptedKey = key;
            else
                article.EncryptedKey = null;
        }
        
        var friends = await friendRepository.GetAllFriends(user.Id);
        resultDto.FriendsCount = friends.Count;
        
        var response = Result<GetProfileDto>.Success(resultDto);
        
        return response;
    }

    public async Task<Result<List<GetUserDto>>> GetBlockUsers(Guid userId)
    {
        var user = await userRepository.GetById(userId);
        if (user == null)
            return Result<List<GetUserDto>>.Failure("User not found", ErrorCode.NotFound);
        
        var blockUsers = await userRepository.GetBlockUsers(user.BlockedUsers);
        var resultDto = mapper.Map<List<GetUserDto>>(blockUsers);
        
        var response = Result<List<GetUserDto>>.Success(resultDto);

        return response;
    }

    public async Task<Result<GetPublicKeysDto>> GetPublicKey(Guid userId)
    {
        var user = await userRepository.GetById(userId);
        if (user == null)
            return Result<GetPublicKeysDto>.Failure("User not found", ErrorCode.NotFound);

        var result = new GetPublicKeysDto
        {
            UserId = userId,
            PublicKey = user.PublicKey,
        };
        
        return Result<GetPublicKeysDto>.Success(result);
    }

    public async Task<Result<List<GetOtherDto>>> GetUsersWithoutMe(Guid userId)
    {
        var user = await userRepository.GetById(userId);
        if (user == null)
            return Result<List<GetOtherDto>>.Failure("User not found", ErrorCode.NotFound);
        
        var users = await userRepository.GetUsersWithoutMe(userId);

        var result = users.Where(u => !user.BlockedUsers.Contains(u.Id)).ToList();
        
        var resulDto = mapper.Map<List<GetOtherDto>>(result);
        
        return Result<List<GetOtherDto>>.Success(resulDto);
    }

    public async Task<Result> Block(ListIdDto blockIds, Guid userId)
    {
        var isValid = await validationService.Validate(blockIds);
        if (!isValid.IsSuccess)
            return Result.Failure(isValid.Error, ErrorCode.InvalidInput);
        
        if (blockIds.Ids.Contains(userId))
            return Result.Failure("Invalid block list", ErrorCode.InvalidInput);
        
        var isExist = await userRepository.ExistsById(userId);
        if (!isExist)
            return Result.Failure("User not found", ErrorCode.NotFound);
        
        foreach (var id in blockIds.Ids)
        {
            isExist = await userRepository.ExistsById(id);
            if (!isExist)
                return Result.Failure("User from list not found", ErrorCode.NotFound);
        }
        
        var uniqueIds = blockIds.Ids.Distinct().ToList();
        
        await userRepository.Block(uniqueIds, userId);
        return Result.Success();
    }

    public async Task<Result> Unblock(ListIdDto unblockIds, Guid userId)
    {
        var isValid = await validationService.Validate(unblockIds);
        if (!isValid.IsSuccess)
            return Result.Failure(isValid.Error, ErrorCode.InvalidInput);
        
        if (unblockIds.Ids.Contains(userId))
            return Result.Failure("Invalid block list", ErrorCode.InvalidInput);
        
        var isExist = await userRepository.ExistsById(userId);
        if (!isExist)
            return Result.Failure("User not found", ErrorCode.NotFound);

        foreach (var id in unblockIds.Ids)
        {
            isExist = await userRepository.ExistsById(id);
            if (!isExist)
                return Result.Failure("User from list not found", ErrorCode.NotFound);
        }
        
        var uniqueIds = unblockIds.Ids.Distinct().ToList();
        
        await userRepository.Unblock(uniqueIds, userId);
        return Result.Success();
    }

    public async Task<Result> Delete(Guid userId)
    {
        var isExists = await userRepository.ExistsById(userId);
        if (!isExists)
            return Result.Failure("User not found", ErrorCode.NotFound);
        
        await userRepository.Delete(userId);
        return Result.Success();
    }
}