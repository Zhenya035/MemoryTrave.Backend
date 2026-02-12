namespace MemoryTrave.Domain.Models;

public class ArticleAccess
{
    public ulong Id { get; set; }
    
    public Guid UserId { get; set; }
    public string EncryptedKey {get; set;} = string.Empty;
    
    public ulong ArticleId { get; set; }
    public Article Article { get; set; }
}