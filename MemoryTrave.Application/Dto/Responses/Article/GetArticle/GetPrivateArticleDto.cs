namespace MemoryTrave.Application.Dto.Responses.Article.GetArticle;

public class GetPrivateArticleDto : GetArticleBaseDto
{
    public string EncryptedPreviewData { get; set; } = string.Empty;
    public string EncryptedData { get; set; } = string.Empty;
    public string EncryptedKey { get; set; } = string.Empty;
}