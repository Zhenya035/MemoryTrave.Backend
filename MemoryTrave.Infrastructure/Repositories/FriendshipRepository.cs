using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryTrave.Infrastructure.Repositories;

public class FriendshipRepository(MemoryTraveDbContext context) : IFriendshipRepository
{
    public async Task<List<Friendship>> GetAllFriends(Guid userId) =>
        await context.Friendships
            .AsNoTracking()
            .Include(f => f.User)
            .Include(f => f.Friend)
            .Where(f => f.UserId == userId || f.FriendId == userId)
            .ToListAsync();

    public async Task<Friendship?> GetById(Guid friendshipId) =>
    await context.Friendships
        .AsNoTracking()
        .FirstOrDefaultAsync(f => f.Id == friendshipId);

    public async Task Add(Friendship friendship)
    {
        await context.Friendships.AddAsync(friendship);
        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid friendshipId) =>
        await context.Friendships
            .Where(f => f.Id == friendshipId)
            .ExecuteDeleteAsync();
    
    public async Task<bool> ExistByUsers(Guid userId, Guid anotherUserId) =>
        await context.Friendships
            .AsNoTracking()
            .AnyAsync(f => 
                (f.UserId == userId && f.FriendId == anotherUserId) ||
                (f.UserId == anotherUserId && f.FriendId == userId));
}