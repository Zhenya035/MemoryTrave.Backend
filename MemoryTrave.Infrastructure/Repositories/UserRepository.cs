using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryTrave.Infrastructure.Repositories;

public class UserRepository(MemoryTraveDbContext context) : IUserRepository
{
    public async Task<User> Registration(User user)
    {
        var newUser = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        return newUser.Entity;
    }

    public async Task<User?> GetByEmail(string email) =>
        await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<string?> GetKeyById(Guid id)
    {
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        return user?.EncryptedPrivateKey;
    }

    public async Task AddKey(Guid userId, string publicKey, string encyptedPrivateKey)
    { 
        await context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(updSetBuild => updSetBuild
                .SetProperty(u => u.PublicKey, publicKey)
                .SetProperty(u => u.EncryptedPrivateKey, encyptedPrivateKey));
    }

    public async Task<bool> UserExistsById(Guid id) =>
        await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id) != null;

    public async Task<bool> UserExistsByEmail(string email) =>
        await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email) != null;
}