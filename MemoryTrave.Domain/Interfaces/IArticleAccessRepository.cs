using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IArticleAccessRepository
{
    public Task AddList(List<ArticleAccess> articleAccesses);
    
    public Task Sync(Guid articleId, List<ArticleAccess> articleAccesses);
    
    public Task DeleteForArticle(Guid id);
    
    public Task<bool> Exists(Guid articleId, Guid userId);
}