using MemoryTrave.Domain.Enums;

namespace MemoryTrave.Domain.Models;

public class Article
{
    public Guid Id { get; set; }

    public VisibilityEnum Visibility { get; set; } = VisibilityEnum.Private;
    public DateTime LastChange { get; set; } = DateTime.UtcNow;

    public string? EncryptedPreviewData { get; set; } = null;
    public string? EncryptedData { get; set; } = null;
    public List<ArticleAccess>? EncryptedKeys { get; set; } = null;

    public string? Description { get; set; } = null;
    public List<string>? PhotosUrls { get; set; } = null;
    
    public User Author { get; set; }
    public Guid AuthorId { get; set; }
    
    public Location Location { get; set; }
    public Guid LocationId { get; set; }
}