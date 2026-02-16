namespace MemoryTrave.Domain.Models;

public class FriendRequest
{
    public Guid Id { get; set; }
    
    public Guid FromUserId { get; set; }
    public User FromUser { get; set; }
    
    public Guid ToUserId { get; set; }
    public User ToUser { get; set; }
}