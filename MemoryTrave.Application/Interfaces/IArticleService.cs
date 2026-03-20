using MemoryTrave.Application.Dto;
using MemoryTrave.Application.Dto.Photo;
using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Dto.Responses.Article;
using MemoryTrave.Domain.Common;

namespace MemoryTrave.Application.Interfaces;

public interface IArticleService
{
    public Task<Result<GetArticleDto>> GetByIdWithIncludes(Guid articleId, Guid userId);
    
    public Task<Result<IdDto>> AddPrivate(Guid locationId, Guid authorId);
    public Task<Result<IdDto>> AddPublic(AddPublicArticleDto dto, Guid authorId);
    
    public Task<Result> AddDataToPrivate(AddPrivateArticleDto dto, Guid articleId);
    
    public Task<Result> Update(UpdateArticleDto dto, Guid articleId);
   
    public Task<Result> Delete(Guid articleId);
}