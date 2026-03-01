using MemoryTrave.Application.Dto.Requests.Article.Access;
using MemoryTrave.Domain.Enums;

namespace MemoryTrave.Application.Dto.Requests.Article;

public class UpdateArticleDto
{
    public VisibilityEnum  Visibility { get; set; }
    
    public string? EncryptedPreviewData { get; set; }
    public string? EncryptedData { get; set; }
    public List<AddAccessDto>? EncryptedKeys { get; set; }
    
    public string? Description { get; set; }
    public List<string>? PhotosUrls { get; set; }
}