namespace Weather.Domain.Entities;

public class WeatherStation : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Guid CountryId { get; set; }
    public Country Country { get; set; } = null!;

    
    // Relationship
    public ICollection<WeatherReading> Readings { get; set; } = new List<WeatherReading>();
}