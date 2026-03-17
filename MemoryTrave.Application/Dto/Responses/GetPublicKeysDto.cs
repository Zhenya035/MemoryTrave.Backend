namespace MemoryTrave.Application.Dto.Responses;

public class GetPublicKeysDto
{
    public Guid FriendId { get; set; }
    public string PublicKey { get; set; }
}