namespace MemoryTrave.Application.Interfaces.Jwt;

public interface IJwtService
{
    string GenerateJwt(Domain.Models.User user);
}