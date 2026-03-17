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
    IMapper mapper) : IFriendshipService
{
    public async Task<Result<List<GetFriendDto>>> GetAll(Guid userId)
    {
        var isExist = await userRepository.ExistsById(userId);
        if (!isExist)
            return Result<List<GetFriendDto>>.Failure("User not found", ErrorCode.NotFound);
            
        var friendship = await repository.GetAllFriends(userId);
        
        var result = mapper.Map<List<GetFriendDto>>(friendship,
            opt => opt.Items["UserId"] = userId);
        
        return Result<List<GetFriendDto>>.Success(result);
    }

    public async Task<Result<List<GetPublicKeysDto>>> GetPublicKeys(Guid userId)
    {
        var isExist = await userRepository.ExistsById(userId);
        if (!isExist)
            return Result<List<GetPublicKeysDto>>.Failure("User not found", ErrorCode.NotFound);
            
        var friendship = await repository.GetAllFriends(userId);
        
        var result = friendship.Select(friend =>
            friend.UserId == userId
                ? new GetPublicKeysDto { FriendId = friend.FriendId, PublicKey = friend.Friend.PublicKey }
                : new GetPublicKeysDto { FriendId = friend.UserId, PublicKey = friend.User.PublicKey }).ToList();

        return Result<List<GetPublicKeysDto>>.Success(result);
    }

    public async Task<Result> Delete(Guid userId, Guid friendshipId)
    {
        var friendship = await repository.GetById(friendshipId);
        if(friendship == null)
            return Result.Failure("Friendship not found", ErrorCode.NotFound);
        if (friendship.UserId != userId && friendship.FriendId != userId)
            return Result.Failure("User is not in the friendsip", ErrorCode.AccessDenied);

        await repository.Delete(friendshipId);
        return Result.Success();
    }
}