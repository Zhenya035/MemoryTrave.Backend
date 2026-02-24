using MemoryTrave.Application.Dto.Responses.Article;
using MemoryTrave.Domain.Enums;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public static class ArticleMapping
{
    public static GetArticleForProfileDto MapToGetArticleForProfileDto(Article article) =>
        new()
        {
            Id = article.Id,
            LocationName = article.Location?.Name,
            LastChange = article.LastChange,
            IsPrivate = article.Visibility == VisibilityEnum.Private,
            EncryptedPreviewData = article.Visibility == VisibilityEnum.Private ?  article.EncryptedPreviewData : null,
            Description = article.Visibility == VisibilityEnum.Public ? article.Description : null
        };
}