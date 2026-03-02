using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetByEmailForAuth(string email);
    public Task<User?> GetByIdWithArticles(Guid userId);
    public Task<User?> GetById(Guid userId);
    public Task<List<User>> GetBlockUsers(List<Guid> userIds);
    public Task<string?> GetKeyById(Guid id);
    public Task<List<User>> GetUsersWithoutMe(Guid userId);
    
    public Task<User> Registration(User user);
    public Task AddKey(Guid userId, string publicKey, string encryptedPrivateKey);

    public Task Block(List<Guid> blockIds, Guid userId);
    public Task Unblock(List<Guid> unblockIds, Guid userId);
    
    public Task Delete(Guid userId);
    
    public Task<bool> ExistsById(Guid id);
    public Task<bool> ExistsByEmail(string email);
}