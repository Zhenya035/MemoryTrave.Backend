namespace MemoryTrave.Application.Interfaces;

public interface IJwtService
{
    string GenerateJwt(Domain.Models.User user);
}