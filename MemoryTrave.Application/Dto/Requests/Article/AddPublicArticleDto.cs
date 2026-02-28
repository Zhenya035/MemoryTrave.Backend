namespace MemoryTrave.Application.Dto.Requests.Article;

public class AddPublicArticleDto
{
    public string Description { get; set; } = string.Empty;
    public List<string> PhotosUrls { get; set; } = [];
    public Guid LocationId { get; set; }
}