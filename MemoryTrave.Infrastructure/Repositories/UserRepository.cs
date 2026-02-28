using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryTrave.Infrastructure.Repositories;

public class UserRepository(MemoryTraveDbContext context) : IUserRepository
{
    public async Task<User?> GetByEmailForAuth(string email) =>
        await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByIdWithArticles(Guid userId) =>
        await context.Users
            .AsNoTracking()
            .Include(u => u.Articles)
            .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<User?> GetById(Guid userId) =>
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

    public async Task<User> Registration(User user)
    {
        var newUser = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        
        return newUser.Entity;
    }
    
    public async Task AddKey(Guid userId, string publicKey, string encryptedPrivateKey)
    { 
        await context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(p => p
                .SetProperty(u => u.PublicKey, publicKey)
                .SetProperty(u => u.EncryptedPrivateKey, encryptedPrivateKey));
    }

    public async Task Delete(Guid userId)
    {
        await context.FriendRequests
            .Where(fr => fr.FromUserId == userId || fr.ToUserId == userId)
            .ExecuteDeleteAsync();
        
        await context.Friendships
            .Where(fs => fs.UserId == userId || fs.FriendId == userId)
            .ExecuteDeleteAsync();
        
        await context.ArticleAccesses
            .Where(aa => aa.UserId == userId)
            .ExecuteDeleteAsync();
        
        await context.Users
            .Where(u => u.Id == userId)
            .ExecuteDeleteAsync();
    }

    public async Task<bool> ExistsById(Guid id) =>
        await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id) != null;

    public async Task<bool> ExistsByEmail(string email) =>
        await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email) != null;
}