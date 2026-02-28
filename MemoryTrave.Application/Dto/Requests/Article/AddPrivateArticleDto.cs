using MemoryTrave.Application.Dto.Requests.Article.Access;

namespace MemoryTrave.Application.Dto.Requests.Article;

public class AddPrivateArticleDto
{
    public string EncryptedPreviewData { get; set; } = string.Empty;
    public string EncryptedData { get; set; } = string.Empty;
    public List<AddAccessDto> EncryptedKeys { get; set; } = [];
    public Guid LocationId { get; set; }
}