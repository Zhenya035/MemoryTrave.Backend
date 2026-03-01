using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces.Friend;

public interface IFriendRequestService
{
    public Task<Result<List<GetFriendRequestDto>>> GetAllByUserId(Guid userId);
    
    public Task<Result> Create(Guid fromId, Guid toId);
    
    public Task<Result> Confirm(Guid userId, Guid requestId);
    public Task<Result> Cancel(Guid userId, Guid requestId);
}