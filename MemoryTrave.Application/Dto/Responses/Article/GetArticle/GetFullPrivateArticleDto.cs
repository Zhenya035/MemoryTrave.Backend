namespace MemoryTrave.Application.Dto.Responses.Article.GetArticle;

public class GetFullPrivateArticleDto : GetArticleBaseDto
{
    public string EncryptedData { get; set; } = string.Empty;
    public string EncryptedKey { get; set; } = string.Empty;
}