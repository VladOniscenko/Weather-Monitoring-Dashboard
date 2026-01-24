namespace Weather.Domain.Entities;

public class WeatherStation : BaseEntity
{
    public string Name { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTime LastSyncedAt { get; private set; }

    // Relationships
    public Guid CountryId { get; private set; }
    public virtual Country? Country { get; private set; } // Nullable for EF lazy loading

    private readonly List<WeatherReading> _readings = new();
    public virtual IReadOnlyCollection<WeatherReading> Readings => _readings.AsReadOnly();

    // Constructor
    public WeatherStation(string name, double latitude, double longitude, Guid countryId)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Station name required.");
        if (countryId == Guid.Empty) throw new ArgumentException("Station must belong to a country.");

        // Coordinates Validation
        if (latitude < -90 || latitude > 90) throw new ArgumentException("Invalid Latitude.");
        if (longitude < -180 || longitude > 180) throw new ArgumentException("Invalid Longitude.");

        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        CountryId = countryId;
    }

    public void UpdateDetails(string name, double latitude, double longitude, Guid countryId)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        CountryId = countryId;
        RegisterUpdate();
    }

    public void SyncedStation()
    {
        LastSyncedAt = DateTime.UtcNow;
        RegisterUpdate();
    }

    protected WeatherStation() { } // for EF
}