namespace MemoryTrave.Application.Dto.Responses.Friend;

public class GetPublicKeysDto
{
    public Guid FriendId { get; set; }
    public string PublicKey { get; set; }
}