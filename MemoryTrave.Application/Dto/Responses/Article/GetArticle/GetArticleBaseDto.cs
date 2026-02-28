using MemoryTrave.Domain.Enums;

namespace MemoryTrave.Application.Dto.Responses.Article.GetArticle;

public class GetArticleBaseDto
{
    public Guid Id { get; set; }
    public VisibilityEnum Visibility { get; set; }
    public DateTime LastChange { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
}