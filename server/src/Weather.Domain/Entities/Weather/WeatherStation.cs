namespace Weather.Domain.Entities;

public class WeatherStation : BaseEntity
{
    public string Name { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTime LastSyncedAt { get; private set; }

    // Relationships
    public Guid CityId { get; private set; }
    public virtual City? City { get; private set; } // Nullable for EF lazy loading

    private readonly List<WeatherReading> _readings = new();
    public virtual IReadOnlyCollection<WeatherReading> Readings => _readings.AsReadOnly();

    // Constructor
    public WeatherStation(string name, double latitude, double longitude, Guid cityId)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Station name required.");
        if (cityId == Guid.Empty) throw new ArgumentException("Station must belong to a city.");

        // Coordinates Validation
        if (latitude < -90 || latitude > 90) throw new ArgumentException("Invalid Latitude.");
        if (longitude < -180 || longitude > 180) throw new ArgumentException("Invalid Longitude.");

        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        CityId = cityId;
        LastSyncedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, double latitude, double longitude, Guid cityId)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        CityId = cityId;
        RegisterUpdate();
    }

    public void SyncedStation()
    {
        LastSyncedAt = DateTime.UtcNow;
        RegisterUpdate();
    }

    protected WeatherStation() { } // for EF
}