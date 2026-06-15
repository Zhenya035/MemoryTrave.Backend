namespace MemoryTrave.Application.Dto.Responses.User;

public class AuthorizationResponseDto
{
    public string JwtToken { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}