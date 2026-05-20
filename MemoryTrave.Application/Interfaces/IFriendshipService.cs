using MemoryTrave.Application.Dto.Responses;
using MemoryTrave.Application.Dto.Responses.Friend;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces;

public interface IFriendshipService
{
    public Task<Result<List<GetOtherDto>>> GetAll(Guid userId);
    public Task<Result<List<GetOtherDto>>> GetAllWithFriendId(Guid userId);
    public Task<Result<List<GetPublicKeysDto>>> GetPublicKeys(Guid userId);
    
    public Task<Result> Delete(Guid userId, Guid friendshipId);
}