using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IFriendRequestRepository
{
    public Task<List<FriendRequest>> GetAllToUserId(Guid userId);
    public Task<List<FriendRequest>> GetAllFromUserId(Guid userId);
    public Task<List<Guid>> GetAllRequestIds(Guid userId);
    public Task<FriendRequest?> GetById(Guid requestId);
    
    public Task Create(FriendRequest request);
    
    public Task Delete(Guid requestId);
    
    public Task<bool> IsExistsByUsers(Guid userId, Guid anotherUserId);
}