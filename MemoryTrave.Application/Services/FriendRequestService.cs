using AutoMapper;
using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Application.Interfaces.Friend;
using MemoryTrave.Domain.Common;
using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Services;

public class FriendRequestService(
    IFriendRequestRepository repository, 
    IUserRepository userRepository,
    IFriendshipRepository friendshipRepository,
    IMapper mapper) : IFriendRequestService
{
    public async Task<Result<List<GetFriendRequestDto>>> GetAllByUserId(Guid userId)
    {
        var userExist = await userRepository.ExistsById(userId);
        if (!userExist)
            return Result<List<GetFriendRequestDto>>.Failure("User not found", ErrorCode.NotFound);
        
        var requests = await repository.GetAllByUserId(userId);
        var result = mapper.Map<List<GetFriendRequestDto>>(requests);
        
        return Result<List<GetFriendRequestDto>>.Success(result);
    }

    public async Task<Result> Create(Guid fromId, Guid toId)
    {
        if(toId == Guid.Empty)
            return Result.Failure("Id cannot be empty", ErrorCode.NotFound);
        if (fromId == toId)
            return Result.Failure("User cannot send a request to himself", ErrorCode.InvalidInput);
        
        var requestExist = await repository.IsExistsByUsers(fromId, toId);
        if (requestExist)
            return Result.Failure("Request is already created", ErrorCode.AlreadyExists);

        var friendship = await friendshipRepository.ExistByUsers(fromId, toId);
        if (friendship)
            return Result.Failure("Friendship already added", ErrorCode.AlreadyExists);
        
        var request = new FriendRequest
        {
            Id = Guid.NewGuid(),
            FromUserId = fromId,
            ToUserId = toId
        };
        await repository.Create(request);
        return Result.Success();
    }

    public async Task<Result> Confirm(Guid userId, Guid requestId)
    {
        var request =  await repository.GetById(requestId);
        if(request == null)
            return Result.Failure("Request not found", ErrorCode.NotFound);
        if(request.ToUserId != userId)
            return Result.Failure("User is not the recipient", ErrorCode.AccessDenied);

        var friendshipExist = await friendshipRepository.ExistByUsers(request.FromUserId, request.ToUserId);
        if (friendshipExist)
            return Result.Failure("Friendship already added", ErrorCode.AlreadyExists);

        var friendship = new Friendship
        {
            UserId = request.FromUserId,
            FriendId = request.ToUserId
        };

        await friendshipRepository.Add(friendship);
        
        await repository.Delete(requestId);
        return Result.Success();
    }

    public async Task<Result> Cancel(Guid userId, Guid requestId)
    {
        var request =  await repository.GetById(requestId);
        if(request == null)
            return Result.Failure("Request not found", ErrorCode.NotFound);
        if(request.FromUserId != userId && request.ToUserId != userId)
            return Result.Failure("User is not in the request", ErrorCode.AccessDenied);
        
        await repository.Delete(requestId);
        return Result.Success();
    }
}