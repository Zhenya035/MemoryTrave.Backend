namespace MemoryTrave.Application.Dto.Requests.User;

public class AddKeysDto
{
    public string PublicKey { get; set; } = string.Empty;
    public string EncryptedPrivateKey { get; set; } = string.Empty;
}