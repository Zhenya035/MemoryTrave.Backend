using MemoryTrave.Domain.Models;

namespace MemoryTrave.Domain.Interfaces;

public interface ILocationRepository
{
    public Task<List<Location>> GetAll();
    public Task<Location?> Get(Guid locationId, Guid userId);
    
    public Task Add(Location location);
    
    public Task Update(Location location, Guid locationId);
    
    public Task Delete(Guid locationId);
    
    public Task<bool> Exists(Guid locationId);
}