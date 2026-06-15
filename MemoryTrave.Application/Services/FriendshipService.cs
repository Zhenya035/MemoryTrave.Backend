using AutoMapper;
using MemoryTrave.Application.Dto.Responses;
using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Domain.Common;
using MemoryTrave.Domain.Interfaces;

namespace MemoryTrave.Application.Services;

public class FriendshipService(
    IFriendshipRepository repository,
    IUserRepository userRepository,
    IArticleRepository articleRepository,
    IArticleAccessRepository articleAccessRepository,
    IMapper mapper) : IFriendshipService
{
    public async Task<Result<List<GetOtherDto>>> GetAll(Guid userId)
    {
        var isExist = await userRepository.ExistsById(userId);
        if (!isExist)
            return Result<List<GetOtherDto>>.Failure("User not found", ErrorCode.NotFound);
            
        var friendship = await repository.GetAllFriends(userId);
        
        var result = mapper.Map<List<GetOtherDto>>(friendship,
            opt => opt.Items["UserId"] = userId);
        
        return Result<List<GetOtherDto>>.Success(result);
    }

    public async Task<Result<List<GetOtherDto>>> GetAllWithFriendId(Guid userId)
    {
        var isExist = await userRepository.ExistsById(userId);
        if (!isExist)
            return Result<List<GetOtherDto>>.Failure("User not found", ErrorCode.NotFound);
            
        var friendship = await repository.GetAllFriends(userId);
        
        var result = mapper.Map<List<GetOtherDto>>(friendship,
            opt => opt.Items["UserId"] = userId);
        
        foreach (var friend in result)
        {
            var first = friendship.First(u => u.Id == friend.Id);
            friend.Id = first.UserId == userId ? first.FriendId : first.UserId;
        }
        
        return Result<List<GetOtherDto>>.Success(result);
    }

    public async Task<Result<List<GetPublicKeysDto>>> GetPublicKeys(Guid userId)
    {
        var isExist = await userRepository.ExistsById(userId);
        if (!isExist)
            return Result<List<GetPublicKeysDto>>.Failure("User not found", ErrorCode.NotFound);
            
        var friendship = await repository.GetAllFriends(userId);
        
        var result = friendship.Select(friend =>
            friend.UserId == userId
                ? new GetPublicKeysDto { UserId = friend.FriendId, PublicKey = friend.Friend.PublicKey }
                : new GetPublicKeysDto { UserId = friend.UserId, PublicKey = friend.User.PublicKey }).ToList();

        return Result<List<GetPublicKeysDto>>.Success(result);
    }

    public async Task<Result> Delete(Guid userId, Guid friendshipId)
    {
        var friendship = await repository.GetById(friendshipId);
        if(friendship == null)
            return Result.Failure("Friendship not found", ErrorCode.NotFound);
        if (friendship.UserId != userId && friendship.FriendId != userId)
            return Result.Failure("User is not in the friendsip", ErrorCode.AccessDenied);

        var userArticles = await articleRepository.GetArticlesIdsFromUser(userId);
        var friendId = friendship.UserId == userId ? friendship.FriendId : friendship.UserId;
        var friendArticles = await articleRepository.GetArticlesIdsFromUser(friendId);

        await articleAccessRepository.DeleteForUser(userArticles, friendId);
        await articleAccessRepository.DeleteForUser(friendArticles, userId);
        
        await repository.Delete(friendshipId);
        return Result.Success();
    }
}