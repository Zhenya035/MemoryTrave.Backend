namespace MemoryTrave.Application.Interfaces.Jwt;

public interface ICurrentUserProvider
{
    public Guid GetUserId();
}