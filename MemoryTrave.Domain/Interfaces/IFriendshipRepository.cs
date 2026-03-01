using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IFriendshipRepository
{
    public Task<List<Guid>> GetAllFriends(Guid userId);
    
    public Task Add(Friendship friendship);
    public Task<bool> ExistByUsers(Guid userId, Guid anotherUserId);
}