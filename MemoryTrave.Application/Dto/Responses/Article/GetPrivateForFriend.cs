namespace MemoryTrave.Application.Dto.Responses.Article;

public class GetPrivateForFriend
{
    public Guid ArticleId { get; set; }
    public string EncryptedKey { get; set; }
}