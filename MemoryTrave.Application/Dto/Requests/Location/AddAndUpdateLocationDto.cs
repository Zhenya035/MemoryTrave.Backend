using MemoryTrave.Domain.Enums;

namespace MemoryTrave.Application.Dto.Requests.Location;

public class AddAndUpdateLocationDto
{
    public string Name { get; set; } = string.Empty;
    public LocationTypeEnum Type { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}