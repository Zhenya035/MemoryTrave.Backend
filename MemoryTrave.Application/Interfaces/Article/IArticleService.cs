using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Dto.Responses.Article.GetArticle;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces.Article;

public interface IArticleService
{
    public Task<Result<GetArticleBaseDto>> GetByIdWithIncludes(Guid articleId, Guid userId);
    
    public Task<Result> AddPrivate(AddPrivateArticleDto dto, Guid authorId);
    public Task<Result> AddPublic(AddPublicArticleDto dto, Guid authorId);
    
    public Task<Result> Update(UpdateArticleDto dto, Guid articleId);
   
    public Task<Result> Delete(Guid articleId);
}