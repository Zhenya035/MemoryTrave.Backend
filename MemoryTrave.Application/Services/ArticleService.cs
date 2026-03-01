using AutoMapper;
using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Dto.Responses.Article.GetArticle;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Application.Interfaces.Article;
using MemoryTrave.Domain.Common;
using MemoryTrave.Domain.Enums;
using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Services;

public class ArticleService(
    IArticleRepository repository, 
    IArticleAccessRepository accessRepository,
    IMapper mapper, 
    IValidationService validationService) : IArticleService
{
    public async Task<Result<GetArticleBaseDto>> GetByIdWithIncludes(Guid articleId, Guid userId)
    {
        if (articleId == Guid.Empty)
            return Result<GetArticleBaseDto>.Failure("Invalid article ID", ErrorCode.InvalidInput);
        
        if (userId == Guid.Empty)
            return Result<GetArticleBaseDto>.Failure("Invalid user ID", ErrorCode.InvalidInput);
        
        var isExist = await repository.IsExists(articleId);
        if (!isExist)
            return Result<GetArticleBaseDto>.Failure("Article not found", ErrorCode.NotFound);

        var article = await repository.GetByIdWithIncludes(articleId);
        if (article.Visibility == VisibilityEnum.Public)
        {
            var publicArticleDto = mapper.Map<GetPublicArticleDto>(article);
            return Result<GetArticleBaseDto>.Success(publicArticleDto);
        }

        var access = article.EncryptedKeys.FirstOrDefault(k => k.UserId == userId);
        if (access == null)
            return Result<GetArticleBaseDto>.Failure("Access denied", ErrorCode.Forbidden);
        
        var privateArticleDto = mapper.Map<GetPrivateArticleDto>(article);
        privateArticleDto.EncryptedKey = access.EncryptedKey;
        
        return Result<GetArticleBaseDto>.Success(privateArticleDto);
    }

    public async Task<Result> AddPrivate(AddPrivateArticleDto dto, Guid authorId)
    {
        var validResult = await validationService.Validate(dto);
        if (!validResult.IsSuccess)
            return Result.Failure(validResult.Error, validResult.ErrorCode);
        
        if(dto.EncryptedKeys.All(k => k.UserId != authorId))
            return Result.Failure("Invalid keys", ErrorCode.InvalidInput);
        
        var article = mapper.Map<Article>(dto);
        article.Id = Guid.NewGuid();
        article.Visibility = VisibilityEnum.Private;
        article.CreatedAt = DateTime.UtcNow;
        article.LastChange = DateTime.UtcNow;
        article.AuthorId = authorId;
        
        var articleId = await repository.Add(article);
        
        var encryptedKeys = dto.EncryptedKeys.Select(mapper.Map<ArticleAccess>).ToList();
        foreach (var articleAccess in encryptedKeys)
        {
            articleAccess.Id = Guid.NewGuid();
            articleAccess.ArticleId = articleId;
        }
        await accessRepository.AddList(encryptedKeys);
        
        return Result.Success();
    }

    public async Task<Result> AddPublic(AddPublicArticleDto dto, Guid authorId)
    {
        var validResult = await validationService.Validate(dto);
        if (!validResult.IsSuccess)
            return Result.Failure(validResult.Error, validResult.ErrorCode);
        
        var article = mapper.Map<Article>(dto);
        article.Id = Guid.NewGuid();
        article.Visibility = VisibilityEnum.Public;
        article.CreatedAt = DateTime.UtcNow;
        article.LastChange = DateTime.UtcNow;
        article.AuthorId = authorId;
        
        await repository.Add(article);
        return Result.Success();
    }

    public async Task<Result> Update(UpdateArticleDto dto, Guid articleId)
    {
        var validResult = await validationService.Validate(dto);
        if(!validResult.IsSuccess)
            return Result.Failure(validResult.Error, validResult.ErrorCode);

        var article = await repository.GetByIdWithIncludes(articleId);
        if (article == null)
            return Result.Failure("Article not found", ErrorCode.NotFound);
        
        if (dto.Visibility == VisibilityEnum.Private && dto.EncryptedKeys.All(k => k.UserId != article.AuthorId))
            return Result.Failure("Invalid keys", ErrorCode.InvalidInput);
        
        var upArticle = mapper.Map<Article>(dto);
        upArticle.LastChange = DateTime.UtcNow;
        
        if (upArticle.Visibility == VisibilityEnum.Private)
        {
            upArticle.Description = null;
            upArticle.PhotosUrls = null;

            var encryptedKeys = dto.EncryptedKeys.Select(mapper.Map<ArticleAccess>).ToList();
            foreach (var aa in encryptedKeys)
            {
                aa.Id =  Guid.NewGuid();
                aa.ArticleId = articleId;
            }
            await accessRepository.Sync(articleId, encryptedKeys);
        }
        else
        {
            upArticle.EncryptedPreviewData = null;
            upArticle.EncryptedData = null;

            if (article.Visibility == VisibilityEnum.Private)
            {
                await accessRepository.DeleteForArticle(articleId);
            }
        }
        
        await repository.Update(upArticle, articleId);
        
        return Result.Success();
    }

    public async Task<Result> Delete(Guid articleId)
    {
        var isExist = await repository.IsExists(articleId);
        if (!isExist)
            return Result.Failure("Article not found", ErrorCode.NotFound);
        
        await repository.Delete(articleId);
        return Result.Success();
    }
}