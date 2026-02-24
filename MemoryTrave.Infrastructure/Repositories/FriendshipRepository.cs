using MemoryTrave.Domain.Interfaces;
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
}