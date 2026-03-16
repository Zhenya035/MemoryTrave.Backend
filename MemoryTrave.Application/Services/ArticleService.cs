using AutoMapper;
using MemoryTrave.Application.Dto.Photo;
using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Dto.Responses.Article;
using MemoryTrave.Application.Interfaces;
using MemoryTrave.Domain.Common;
using MemoryTrave.Domain.Enums;
using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Services;

public class ArticleService(
    IArticleRepository repository,
    ILocationRepository locationRepository,
    IArticleAccessRepository accessRepository,
    IMapper mapper, 
    IValidationService validationService) : IArticleService
{
    public async Task<Result<GetArticleDto>> GetByIdWithIncludes(Guid articleId, Guid userId)
    {
        if (articleId == Guid.Empty)
            return Result<GetArticleDto>.Failure("Invalid article ID", ErrorCode.InvalidInput);
        
        if (userId == Guid.Empty)
            return Result<GetArticleDto>.Failure("Invalid user ID", ErrorCode.InvalidInput);
        
        var isExist = await repository.IsExists(articleId);
        if (!isExist)
            return Result<GetArticleDto>.Failure("Article not found", ErrorCode.NotFound);

        var article = await repository.GetByIdWithIncludes(articleId);
        if (article.Visibility == VisibilityEnum.Public)
        {
            var publicArticleDto = mapper.Map<GetArticleDto>(article);
            return Result<GetArticleDto>.Success(publicArticleDto);
        }

        var access = article.EncryptedKeys.FirstOrDefault(k => k.UserId == userId);
        if (access == null)
            return Result<GetArticleDto>.Failure("Access denied", ErrorCode.AccessDenied);
        
        var privateArticleDto = mapper.Map<GetArticleDto>(article);
        privateArticleDto.EncryptedKey = access.EncryptedKey;
        
        return Result<GetArticleDto>.Success(privateArticleDto);
    }

    public async Task<Result<Guid>> AddPrivate(Guid locationId, Guid authorId)
    {
        var isExists = await locationRepository.Exists(locationId);
        if(!isExists)
            return Result<Guid>.Failure("Location not found", ErrorCode.NotFound);

        var article = new Article
        {
            Id = Guid.NewGuid(),
            Visibility = VisibilityEnum.Private,
            CreatedAt = DateTime.UtcNow,
            LastChange = DateTime.UtcNow,
            AuthorId = authorId,
            LocationId = locationId
        };

        var articleId = await repository.Add(article);
        
        return Result<Guid>.Success(articleId);
    }

    public async Task<Result<Guid>> AddPublic(AddPublicArticleDto dto, Guid authorId)
    {
        var validResult = await validationService.Validate(dto);
        if (!validResult.IsSuccess)
            return Result<Guid>.Failure(validResult.Error, validResult.ErrorCode);
        
        var article = mapper.Map<Article>(dto);
        article.Id = Guid.NewGuid();
        article.Visibility = VisibilityEnum.Public;
        article.CreatedAt = DateTime.UtcNow;
        article.LastChange = DateTime.UtcNow;
        article.AuthorId = authorId;
        
        var articleId = await repository.Add(article);
        return Result<Guid>.Success(articleId);
    }

    public async Task<Result> AddDataToPrivate(AddPrivateArticleDto dto, Guid articleId)
    {
        var isValid = await validationService.Validate(dto);
        if (!isValid.IsSuccess && isValid.Error != null)
            return Result.Failure(isValid.Error, ErrorCode.InvalidInput);
        
        var article = await repository.GetByIdWithIncludes(articleId);
        if (article == null)
            return Result.Failure("Article not found", ErrorCode.NotFound);
        if (article.Visibility == VisibilityEnum.Public)
            return Result.Failure("Incorrect visibility", ErrorCode.InvalidInput);
        if (article.EncryptedPreviewData != null || article.EncryptedData != null || article.EncryptedKeys != null)
            return Result.Failure("Already added", ErrorCode.AlreadyExists);

        article.EncryptedPreviewData = dto.EncryptedPreviewData;
        article.EncryptedData = dto.EncryptedData;

        await repository.Update(article, articleId);
        
        var encryptedKeys = dto.EncryptedKeys.Select(mapper.Map<ArticleAccess>).ToList();
        foreach (var articleAccess in encryptedKeys)
        {
            articleAccess.Id = Guid.NewGuid();
            articleAccess.ArticleId = articleId;
        }
        await accessRepository.AddList(encryptedKeys);
        
        return Result.Success();
    }

    public async Task<Result> AddPhotoToPublic(PhotosDto dto, Guid articleId)
    {
        var article = await repository.GetByIdWithIncludes(articleId);
        if (article == null)
            return Result.Failure("Article not found", ErrorCode.NotFound);
        if (article.Visibility == VisibilityEnum.Private)
            return Result.Failure("Incorrect visibility", ErrorCode.InvalidInput);
        if (article.PhotosUrls != null)
            return Result.Failure("Already added", ErrorCode.AlreadyExists);

        article.PhotosUrls = dto.Photos;
        
        await repository.Update(article, articleId);
        
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