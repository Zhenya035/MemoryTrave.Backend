using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface IArticleAccessRepository
{
    public Task AddList(List<ArticleAccess> articleAccesses);
    public Task Add(ArticleAccess articleAccess);
    
    public Task DeleteList(List<Guid> ids);
    public Task Delete(Guid id);
    
    public Task<bool> Exists(Guid articleId, Guid userId);
}