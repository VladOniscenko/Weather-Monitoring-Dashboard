namespace Weather.Domain.Entities;

public class City : BaseEntity
{
    public Guid CountryId { get; private set; }
    public virtual Country? Country { get; private set; } // "virtual" allows EF Core to lazy load

    public string Name { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public long Population { get; private set; }
    public string Timezone { get; private set; }

    public City(Guid countryId, string name, double latitude, double longitude, string timezone)
    {
        if (countryId == Guid.Empty) throw new ArgumentException("City must belong to a country.");
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("City Name is required.");
        
        // Validate Coordinates
        if (latitude < -90 || latitude > 90) throw new ArgumentException("Invalid Latitude.");
        if (longitude < -180 || longitude > 180) throw new ArgumentException("Invalid Longitude.");

        CountryId = countryId;
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Timezone = timezone;
    }

    protected City() { } // for EF
}