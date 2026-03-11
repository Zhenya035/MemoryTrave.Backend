using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Dto.Responses.Article;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces;

public interface IArticleService
{
    public Task<Result<GetArticleDto>> GetByIdWithIncludes(Guid articleId, Guid userId);
    
    public Task<Result> AddPrivate(AddPrivateArticleDto dto, Guid authorId);
    public Task<Result> AddPublic(AddPublicArticleDto dto, Guid authorId);
    
    public Task<Result> Update(UpdateArticleDto dto, Guid articleId);
   
    public Task<Result> Delete(Guid articleId);
}