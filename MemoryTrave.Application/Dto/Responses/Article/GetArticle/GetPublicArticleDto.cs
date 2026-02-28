namespace MemoryTrave.Application.Dto.Responses.Article.GetArticle;

public class GetPublicArticleDto : GetArticleBaseDto
{
    public string Description { get; set; } = string.Empty;
    public List<string> PhotosUrls { get; set; } = [];
}