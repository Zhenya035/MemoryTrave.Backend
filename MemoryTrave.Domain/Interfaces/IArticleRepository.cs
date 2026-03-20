using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IArticleRepository
{
    public Task<Article?> GetByIdWithIncludes(Guid id);
    public Task<Guid> Add(Article article);
    public Task Update(Article article, Guid articleId);
    public Task Delete(Guid articleId);
    
    public Task<bool> IsExists(Guid articleId);
}