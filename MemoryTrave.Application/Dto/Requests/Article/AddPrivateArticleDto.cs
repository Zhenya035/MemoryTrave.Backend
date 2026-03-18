using MemoryTrave.Application.Dto.Requests.Article.Access;

namespace MemoryTrave.Application.Dto.Requests.Article;

public class AddPrivateArticleDto
{
    public string EncryptedDescription { get; set; } = string.Empty;
    public List<AddAccessDto> EncryptedKeys { get; set; } = [];
}