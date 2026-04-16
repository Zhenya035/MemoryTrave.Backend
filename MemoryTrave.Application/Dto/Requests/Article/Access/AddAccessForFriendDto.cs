namespace MemoryTrave.Application.Dto.Requests.Article.Access;

public class AddAccessForFriendDto
{
    public string EncryptedKey {get; set;} = string.Empty;
    public Guid ArticleId { get; set; }
}