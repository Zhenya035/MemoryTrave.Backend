namespace MemoryTrave.Application.Dto.Requests.User;

public class RegistrationDto
{
    public string Email { get; set; } =  string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}