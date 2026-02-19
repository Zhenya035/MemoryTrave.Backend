using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IUserRepository
{
    public Task<User> Registration(User user);
    public Task<User?> GetByEmail(string email);
    public Task<string?> GetKeyById(Guid id);
    
    public Task<bool> UserExistsById(Guid id);
    public Task<bool> UserExistsByEmail(string email);
}