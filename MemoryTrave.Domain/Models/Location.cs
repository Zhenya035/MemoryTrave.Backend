namespace MemoryTrave.Domain.Models;

public class Location
{
    public Guid  Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Geohash { get; set; }

    public List<Article> Articles { get; set; } = [];
}