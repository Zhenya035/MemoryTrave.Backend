using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetByEmailForAuth(string email);
    public Task<User?> GetByIdWithArticles(Guid userId);
    public Task<User?> GetById(Guid userId);
    public Task<List<User>> GetBlockUsers(List<Guid> userIds);
    public Task<string?> GetKeyById(Guid id);
    
    public Task<User> Registration(User user);
    public Task AddKey(Guid userId, string publicKey, string encryptedPrivateKey);

    public Task Delete(Guid userId);
    
    public Task<bool> ExistsById(Guid id);
    public Task<bool> ExistsByEmail(string email);
}