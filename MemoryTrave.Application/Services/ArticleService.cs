using AutoMapper;
using MemoryTrave.Application.Dto;
using MemoryTrave.Application.Dto.Requests.Article;
using MemoryTrave.Application.Dto.Requests.Article.Access;
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
    IUserRepository userRepository,
    IMapper mapper, 
    IValidationService validationService) : IArticleService
{
    public async Task<Result<GetArticleDto>> GetByIdWithIncludes(Guid articleId, Guid userId)
    {
        if (articleId == Guid.Empty)
            return Result<GetArticleDto>.Failure("Invalid article ID", ErrorCode.InvalidInput);
        
        var isExist = await repository.IsExists(articleId);
        if (!isExist)
            return Result<GetArticleDto>.Failure("Article not found", ErrorCode.NotFound);

        var article = await repository.GetByIdWithIncludes(articleId);
        if (article.Visibility == VisibilityEnum.Public)
        {
            var publicArticleDto = mapper.Map<GetArticleDto>(article);
            return Result<GetArticleDto>.Success(publicArticleDto);
        }

        if (userId == Guid.Empty)
            return Result<GetArticleDto>.Failure("Unauthorized", ErrorCode.Unauthorized);

        var access = article.EncryptedKeys.FirstOrDefault(k => k.UserId == userId);
        if (access == null)
            return Result<GetArticleDto>.Failure("Access denied", ErrorCode.AccessDenied);
        
        var privateArticleDto = mapper.Map<GetArticleDto>(article);
        privateArticleDto.EncryptedKey = access.EncryptedKey;
        
        return Result<GetArticleDto>.Success(privateArticleDto);
    }

    public async Task<Result<List<GetPrivateForFriend>>> GetPrivate(Guid userId)
    {
        var articles = await repository.GetPrivate(userId);

        var result = new List<GetPrivateForFriend>();

        foreach (var article in articles)
        {
            var newResult = new GetPrivateForFriend
            {
                ArticleId = article.Id,
                EncryptedKey = article.EncryptedKeys.First(eK => eK.UserId == userId).EncryptedKey
            };

            result.Add(newResult);
        }
        
        return Result<List<GetPrivateForFriend>>.Success(result);
    }

    public async Task<Result<IdDto>> GetAuthor(Guid articleId)
    {
        var isExist = await repository.IsExists(articleId);
        if (!isExist)
            return Result<IdDto>.Failure("Article not found", ErrorCode.NotFound);
        
        var article = await repository.GetByIdWithIncludes(articleId);
        var result = new IdDto { Id = article.AuthorId };
        
        return Result<IdDto>.Success(result);
    }

    public async Task<Result<IdDto>> AddPrivate(Guid locationId, Guid authorId)
    {
        var isExists = await locationRepository.Exists(locationId);
        if(!isExists)
            return Result<IdDto>.Failure("Location not found", ErrorCode.NotFound);

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
        var result = new IdDto { Id = articleId };
        
        return Result<IdDto>.Success(result);
    }

    public async Task<Result<IdDto>> AddPublic(AddPublicArticleDto dto, Guid authorId)
    {
        var validResult = await validationService.Validate(dto);
        if (!validResult.IsSuccess)
            return Result<IdDto>.Failure(validResult.Error, validResult.ErrorCode);
        
        var article = mapper.Map<Article>(dto);
        article.Id = Guid.NewGuid();
        article.Visibility = VisibilityEnum.Public;
        article.CreatedAt = DateTime.UtcNow;
        article.LastChange = DateTime.UtcNow;
        article.AuthorId = authorId;
        
        var articleId = await repository.Add(article);
        var result = new IdDto { Id = articleId };
        
        return Result<IdDto>.Success(result);
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
        
        article.EncryptedDescription = dto.EncryptedDescription;
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

    public async Task<Result> AddAccess(List<AddAccessForFriendDto> dto, Guid userId)
    {
        var isValid = await validationService.Validate(dto);
        if (!isValid.IsSuccess && isValid.Error != null)
            return Result.Failure(isValid.Error, ErrorCode.InvalidInput);

        var userIsExist = await userRepository.ExistsById(userId);
        if (!userIsExist)
            return Result.Failure("User not found", ErrorCode.NotFound);

        var encryptedKeys = dto.Select(access => mapper.Map<ArticleAccess>(access)).ToList();

        foreach (var encryptedKey in encryptedKeys)
        {
            encryptedKey.Id = Guid.NewGuid();
            encryptedKey.UserId = userId;
        }

        await accessRepository.AddList(encryptedKeys);
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