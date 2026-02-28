using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryTrave.Infrastructure.Repositories;

public class ArticleRepository(MemoryTraveDbContext context) : IArticleRepository
{
    public async Task<Article?> GetByIdWithIncludes(Guid id) =>
        await context.Articles
            .AsNoTracking()
            .Include(a => a.EncryptedKeys)
            .Include(a => a.Author)
            .Include(a => a.Location)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<Guid> Add(Article article)
    {
        var newArticle = await context.Articles.AddAsync(article);
        await context.SaveChangesAsync();

        return newArticle.Entity.Id;
    }

    public async Task Update(Article article, Guid articleId) =>
        await context.Articles
            .Where(a => a.Id == articleId)
            .ExecuteUpdateAsync(p => p
                .SetProperty(a => a.Visibility, article.Visibility)
                .SetProperty(a => a.EncryptedPreviewData, article.EncryptedPreviewData)
                .SetProperty(a => a.EncryptedData, article.EncryptedData)
                .SetProperty(a => a.Description, article.Description)
                .SetProperty(a => a.PhotosUrls, article.PhotosUrls)
                .SetProperty(a => a.LastChange, article.LastChange));

    public async Task Delete(Guid articleId) =>
        await context.Articles
            .Where(a => a.Id == articleId)
            .ExecuteDeleteAsync();

    public async Task<bool> IsExists(Guid articleId) =>
        await context.Articles
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == articleId) != null;
}