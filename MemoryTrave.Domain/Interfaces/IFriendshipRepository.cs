using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IFriendshipRepository
{
    public Task<List<Friendship>> GetAllFriends(Guid userId);
    public Task<Friendship?> GetById(Guid friendshipId);
    
    public Task Add(Friendship friendship);
    
    public Task Delete(Guid friendshipId);
    
    public Task<bool> ExistByUsers(Guid userId, Guid anotherUserId);
}