using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IUserRepository
{
    public Task<User> Registration(User user);
    public Task<User?> GetByEmailForAuth(string email);
    public Task<User?> GetByIdWithArticles(Guid userId);
    public Task<User?> GetUserById(Guid userId);
    public Task<List<User>> GetBlockUsers(List<Guid> userIds);
    public Task<string?> GetKeyById(Guid id);
    
    public Task AddKey(Guid userId, string publicKey, string encyptedPrivateKey);
    public Task<bool> UserExistsById(Guid id);
    public Task<bool> UserExistsByEmail(string email);
}