namespace MemoryTrave.Application.Dto.Responses.Friend;

public class GetFriendRequestDto
{
    public Guid Id { get; set; }
    public string FromUserName { get; set; }
    public string ToUserName { get; set; }
}