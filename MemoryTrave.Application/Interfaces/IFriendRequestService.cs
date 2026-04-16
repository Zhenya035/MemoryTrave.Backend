using MemoryTrave.Application.Dto;
using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces;

public interface IFriendRequestService
{
    public Task<Result<List<GetFriendDto>>> GetAllToUserId(Guid userId);
    public Task<Result<List<GetFriendDto>>> GetAllFromUserId(Guid userId);
    
    public Task<Result> Create(Guid fromId, Guid toId);
    
    public Task<Result<IdDto>> Confirm(Guid userId, Guid requestId);
    public Task<Result> Cancel(Guid userId, Guid requestId);
}