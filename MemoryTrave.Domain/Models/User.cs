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
    
    public List<FriendRequest> FriendRequests { get; set; } = [];
    public List<Friendship> Friendships { get; set; } = [];
    
    public List<Article> Articles { get; set; } = [];
}