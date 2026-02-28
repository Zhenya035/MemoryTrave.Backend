using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryTrave.Infrastructure.Repositories;

public class ArticleAccessRepository(MemoryTraveDbContext context) : IArticleAccessRepository
{
    public async Task AddList(List<ArticleAccess> articleAccesses)
    {
        await context.ArticleAccesses
            .AddRangeAsync(articleAccesses);
        await context.SaveChangesAsync();
    }

    public async Task Sync(Guid articleId, List<ArticleAccess> articleAccesses)
    {
        var existingAccesses = await context.ArticleAccesses
            .AsNoTracking()
            .Where(a => a.ArticleId == articleId)
            .ToListAsync();
        
        var existingUsersIds = new HashSet<Guid>(existingAccesses.Select(a => a.UserId));
        var desiredUsersIds = new HashSet<Guid>(articleAccesses.Select(a => a.UserId));
        
        var usersToDelete = existingUsersIds.Except(desiredUsersIds).ToList();
        if (usersToDelete.Count != 0)
            await context.ArticleAccesses
                .Where(a => a.ArticleId == articleId && usersToDelete.Contains(a.UserId))
                .ExecuteDeleteAsync();
        
        var usersToAdd = articleAccesses
            .Where(a => !existingUsersIds.Contains(a.UserId))
            .ToList();

        if (usersToAdd.Count != 0)
            await context.ArticleAccesses.AddRangeAsync(usersToAdd);
        
        await context.SaveChangesAsync();
    }

    public async Task DeleteForArticle(Guid articleId) =>
        await context.ArticleAccesses
            .Where(a => a.ArticleId == articleId)
            .ExecuteDeleteAsync();

    public async Task<bool> Exists(Guid articleId, Guid userId) =>
    await context.ArticleAccesses
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.UserId == userId && a.ArticleId == articleId) != null;
}