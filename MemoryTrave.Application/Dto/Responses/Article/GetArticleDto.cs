using MemoryTrave.Domain.Enums;

namespace MemoryTrave.Application.Dto.Responses.Article;

public class GetArticleDto
{
    public Guid Id { get; set; }
    public VisibilityEnum Visibility { get; set; }
    public DateTime LastChange { get; set; }
    public DateTime CreatedAt{ get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string LocationName { get; set; } = string.Empty;
    
    public string? EncryptedDescription { get; set; }
    public string? EncryptedKey { get; set; }
    
    public string? Description { get; set; }
    public List<string>? PhotosUrls { get; set; }
}