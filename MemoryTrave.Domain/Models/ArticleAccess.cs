namespace MemoryTrave.Domain.Models;

public class ArticleAccess
{
    public Guid Id { get; set; }
    
    public string EncryptedKey {get; set;} = string.Empty;
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public Guid ArticleId { get; set; }
    public Article Article { get; set; }
}