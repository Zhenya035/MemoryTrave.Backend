namespace MemoryTrave.Application.Dto.Responses.Article.Access;

public class GetArticleAccessDto
{
    public string EncryptedKey {get; set;} = string.Empty;
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
}