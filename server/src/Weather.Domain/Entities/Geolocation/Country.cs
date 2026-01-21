namespace Weather.Domain.Entities;

public class Country : BaseEntity
{
    public string Name { get; private set; }
    public string CCA2 { get; private set; } // ISO Code ("NL")
    public string CCA3 { get; private set; } // ISO Code ("NLD")
    public string Region { get; private set; }
    public string Subregion { get; private set; }
    public string Capital { get; private set; }
    public string Flag { get; private set; } // Emoji

    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public bool Independent { get; private set; }
    public bool Landlocked { get; private set; }

    private readonly List<City> _cities = new();
    public virtual IReadOnlyCollection<City> Cities => _cities.AsReadOnly();

    public Country(
            string name,
            string cca2,
            string cca3,
            string region,
            string subregion,
            string capital,
            string flag,
            double latitude,
            double longitude,
            bool independent,
            bool landlocked)
    {
        UpdateDetails(name, cca2, cca3, region, subregion, capital, flag, latitude, longitude, independent, landlocked);
    }

    public void UpdateDetails(
        string name,
        string cca2,
        string cca3,
        string region,
        string subregion,
        string capital,
        string flag,
        double latitude,
        double longitude,
        bool independent,
        bool landlocked)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentException("Country name is required.");

        if (string.IsNullOrWhiteSpace(cca2) || cca2.Length != 2) 
            throw new ArgumentException($"CCA2 code must be exactly 2 characters. Received: {cca2}");

        if (string.IsNullOrWhiteSpace(cca3) || cca3.Length != 3) 
            throw new ArgumentException($"CCA3 code must be exactly 3 characters. Received: {cca3}");

        if (latitude < -90 || latitude > 90) 
            throw new ArgumentException($"Invalid latitude: {latitude}.");

        if (longitude < -180 || longitude > 180) 
            throw new ArgumentException($"Invalid longitude: {longitude}.");

        Name = name.Trim();
        CCA2 = cca2.ToUpperInvariant();
        CCA3 = cca3.ToUpperInvariant();
        Region = region?.Trim() ?? string.Empty;
        Subregion = subregion?.Trim() ?? string.Empty;
        Capital = capital?.Trim() ?? string.Empty;
        Flag = flag?.Trim() ?? string.Empty;
        Latitude = latitude;
        Longitude = longitude;
        Independent = independent;
        Landlocked = landlocked;

        RegisterUpdate(); 
    }

    // EF Core needs this
    protected Country() { }
}