using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MemoryTrave.Application.Services;
using MemoryTrave.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MemoryTrave.Infrastructure.Jwt;

public class JwtService(IOptions<JwtSettings> settings) : IJwtService
{
    private readonly JwtSettings _settings = settings.Value;
    
    public string GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new("email", user.Email),
            new("role", user.Role.ToString()),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));

        var jwtToken = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            expires: DateTime.UtcNow.Add(_settings.Expires),
            claims: claims,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}