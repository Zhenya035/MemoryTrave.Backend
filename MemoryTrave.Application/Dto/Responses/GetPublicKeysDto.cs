namespace MemoryTrave.Application.Dto.Responses;

public class GetPublicKeysDto
{
    public Guid UserId { get; set; }
    public string PublicKey { get; set; }
}