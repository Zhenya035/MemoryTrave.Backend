namespace MemoryTrave.Domain.Interfaces;

public interface IFriendshipRepository
{
    public Task<List<Guid>> GetAllFriends(Guid userId);
}