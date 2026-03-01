using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryTrave.Infrastructure.Repositories;

public class FriendshipRepository(MemoryTraveDbContext context) : IFriendshipRepository
{
    public async Task<List<Guid>> GetAllFriends(Guid userId) =>
        await context.Friendships
            .AsNoTracking()
            .Where(f => f.UserId == userId || f.FriendId == userId)
            .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
            .ToListAsync();

    public async Task Add(Friendship friendship)
    {
        await context.Friendships.AddAsync(friendship);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ExistByUsers(Guid userId, Guid anotherUserId) =>
        await context.Friendships
            .AnyAsync(f => 
                (f.UserId == userId && f.FriendId == anotherUserId) ||
                (f.UserId == anotherUserId && f.FriendId == userId));
}