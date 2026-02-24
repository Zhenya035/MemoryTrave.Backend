using System.Security.Claims;
using MemoryTrave.Application.Interfaces.Jwt;
using MemoryTrave.Domain.Exceptions;

namespace MemoryTrave.Web;

public class CurrentUserProvider(IHttpContextAccessor accessor) : ICurrentUserProvider
{
    public Guid GetUserId()
    {
        var user = accessor.HttpContext?.User;
        
        if(user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            throw new InvalidInputDataException("Token is invalid");

        var idClaim = user.FindFirst("id") ?? user.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim == null || !Guid.TryParse(idClaim.Value, out var userId))
            throw new NotFoundException("UserId");
        
        return userId;
    }
}