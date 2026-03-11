using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces;

public interface IFriendshipService
{
    public Task<Result<List<GetFriendDto>>> GetAll(Guid userId);
    public Task<Result> Delete(Guid userId, Guid friendshipId);
}