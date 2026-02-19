namespace MemoryTrave.Application.Interfaces;

public interface ICurrentUserProvider
{
    public Guid GetUserId();
}