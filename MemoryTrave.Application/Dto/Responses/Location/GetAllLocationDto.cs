using MemoryTrave.Domain.Enums;

namespace MemoryTrave.Application.Dto.Responses.Location;

public class GetAllLocationDto
{
    public Guid  Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public LocationTypeEnum LocationType { get; set; }
    public LocationContentState LocationContentState { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Geohash { get; set; } =  string.Empty;
}