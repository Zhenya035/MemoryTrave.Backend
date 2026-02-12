namespace MemoryTrave.Domain.Models;

public class Article
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public string? EncryptedData { get; set; } = null;
    public List<ArticleAccess>? EncryptedDecs { get; set; } = null;

    public string? Description { get; set; } = null;
    public List<string>? PhotosUrls { get; set; } = null;
    public bool IsPublic { get; set; } = true;
    
    public User Author { get; set; }
    public Guid AuthorId { get; set; }
    
    public Location Location { get; set; }
    public Guid LocationId { get; set; }
}