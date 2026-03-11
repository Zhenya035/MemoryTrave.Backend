using MemoryTrave.Domain.Enums;
using MemoryTrave.Domain.Interfaces;
using MemoryTrave.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MemoryTrave.Infrastructure.Repositories;

public class LocationRepository(MemoryTraveDbContext context) : ILocationRepository
{
    public async Task<List<Location>> GetAll() =>
        await context.Locations
            .AsNoTracking()
            .ToListAsync();

    public async Task<Location?> Get(Guid locationId, Guid userId) =>
        await context.Locations
            .AsNoTracking()
            .Include(l => l.Articles)
                .ThenInclude(a => a.EncryptedKeys)
            .Include(l => l.Articles)
                .ThenInclude(a => a.Author)
            .Include(l => l.Articles)
                .ThenInclude(a => a.Location)
            .Select(l => new Location
            {
                Id = l.Id,
                Name = l.Name,
                Type = l.Type,
                Latitude = l.Latitude,
                Longitude = l.Longitude,
                Geohash = l.Geohash,
                Articles = l.Articles
                    .Where(a => a.Visibility == VisibilityEnum.Public ||
                                (a.Visibility == VisibilityEnum.Private &&
                                 a.EncryptedKeys.Any(k => k.UserId == userId)))
                    .ToList()
            })
            .FirstOrDefaultAsync(l => l.Id == locationId);

    public async Task Add(Location location)
    {
        await context.Locations.AddAsync(location);
        await context.SaveChangesAsync();
    }

    public async Task Update(Location location, Guid locationId) =>
        await context.Locations
            .Where(l => l.Id == locationId)
            .ExecuteUpdateAsync(p => p
                .SetProperty(l => l.Name, location.Name)
                .SetProperty(l => l.Latitude, location.Latitude)
                .SetProperty(l => l.Longitude, location.Longitude)
                .SetProperty(l => l.Geohash, location.Geohash));

    public async Task Delete(Guid locationId) =>
        await context.Locations
            .Where(l => l.Id == locationId)
            .ExecuteDeleteAsync();

    public Task<bool> Exists(Guid locationId) =>
        context.Locations
            .AnyAsync(l => l.Id == locationId);
}