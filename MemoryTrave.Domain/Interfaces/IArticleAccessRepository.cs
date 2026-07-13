using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IArticleAccessRepository
{
    public Task<List<Guid>> GetFriendsByArticle(Guid articleId);
    
    public Task AddList(List<ArticleAccess> articleAccesses);
    
    public Task Sync(Guid articleId, List<ArticleAccess> articleAccesses);
    
    public Task DeleteForArticle(Guid articleId);
    public Task DeleteForUser(List<Guid> articleIds, Guid userId);
    
    public Task<bool> Exists(Guid articleId, Guid userId);
}