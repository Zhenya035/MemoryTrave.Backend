namespace MemoryTrave.Application.Dto.Responses.Article.GetArticle;

public class GetPreviewPrivateArticle : GetArticleBaseDto
{
    public string EncryptedPreviewData { get; set; } = string.Empty;
}