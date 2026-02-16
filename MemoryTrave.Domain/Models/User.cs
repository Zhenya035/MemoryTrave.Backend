using MemoryTrave.Domain.Enums;

namespace MemoryTrave.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    
    public string Username { get; set; } = string.Empty;
    public string EmailHash { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public RoleEnum Role { get; set; } = RoleEnum.User;
    
    public List<FriendRequest> SentFriendRequests { get; set; } = [];
    public List<FriendRequest> ReceivedFriendRequests { get; set; } = [];
    
    public List<Guid> BlockedUsers { get; set; } = [];
    public List<ArticleAccess> ArticleAccesses { get; set; } = [];

    public List<Article> Articles { get; set; } = [];
    
    public DateTime? BanExpiresAt { get; set; } = null;
    public bool IsBlocked => BanExpiresAt > DateTime.UtcNow;
}