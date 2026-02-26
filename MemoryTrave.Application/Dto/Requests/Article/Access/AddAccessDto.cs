namespace MemoryTrave.Application.Dto.Requests.Article.Access;

public class AddAccessDto
{
    public string EncryptedKey {get; set;} = string.Empty;
    public Guid UserId { get; set; }
    public Guid ArticleId { get; set; }
}