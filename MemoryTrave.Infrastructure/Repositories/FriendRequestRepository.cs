using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryTrave.Infrastructure.Repositories;

public class FriendRequestRepository(MemoryTraveDbContext context) : IFriendRequestRepository
{
    public async Task<List<FriendRequest>> GetAllToUserId(Guid userId) =>
        await context.FriendRequests
            .AsNoTracking()
            .Include(fr => fr.FromUser)
            .Where(fr => fr.ToUserId == userId)
            .ToListAsync();

    public async Task<List<FriendRequest>> GetAllFromUserId(Guid userId) =>
        await context.FriendRequests
            .AsNoTracking()
            .Include(fr => fr.ToUser)
            .Where(fr => fr.FromUserId == userId)
            .ToListAsync();

    public async Task<List<Guid>> GetAllRequestIds(Guid userId) =>
        await context.FriendRequests
            .AsNoTracking()
            .Where(fr => fr.FromUserId == userId || fr.ToUserId == userId)
            .Select(fr => fr.FromUserId == userId ? fr.ToUserId : fr.FromUserId)
            .ToListAsync();

    public async Task<FriendRequest?> GetById(Guid requestId) =>
        await context.FriendRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(fr => fr.Id == requestId);

    public async Task Create(FriendRequest request)
    {
        await context.FriendRequests.AddAsync(request);
        await context.SaveChangesAsync();
    }

    public async Task Delete(Guid requestId) =>
        await context.FriendRequests
            .Where(fr => fr.Id == requestId)
            .ExecuteDeleteAsync();

    public async Task<bool> IsExistsByUsers(Guid userId, Guid anotherUserId) =>
    await context.FriendRequests
        .AsNoTracking()
        .FirstOrDefaultAsync(fr => 
            (fr.FromUserId == userId && fr.ToUserId == anotherUserId) ||
            (fr.FromUserId == anotherUserId && fr.ToUserId == userId)) != null;
}