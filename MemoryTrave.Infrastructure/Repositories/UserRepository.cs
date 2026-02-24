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

    public async Task<User?> GetByEmailForAuth(string email) =>
        await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByIdWithArticles(Guid userId) =>
        await context.Users
            .AsNoTracking()
            .Include(u => u.Articles)
            .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User?> GetUserById(Guid userId) =>
        await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<List<User>> GetBlockUsers(List<Guid> userIds) =>
        await context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();


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