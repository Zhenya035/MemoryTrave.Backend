using System.Security.Claims;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Services;

public interface IJwtService
{
    string GenerateJwt(User user);
}