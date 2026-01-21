namespace Weather.Domain.Entities;

public class WeatherReading : BaseEntity
{
    // Relationship
    public Guid StationId { get; private set; }
    public virtual WeatherStation? Station { get; private set; }

    // Weather Description
    public string MainCondition { get; private set; } // "Clouds"
    public string Description { get; private set; }   // "scattered clouds"
    public string Icon { get; private set; }

    // Main Stats
    public double Temperature { get; private set; }
    public double FeelsLike { get; private set; }
    public double MinTemp { get; private set; }
    public double MaxTemp { get; private set; }
    public int Pressure { get; private set; }
    public int Humidity { get; private set; }
    public int SeaLevel { get; private set; }
    public int GroundLevel { get; private set; }
    public int Visibility { get; private set; }
    public double Min { get; private set; }
    public double Max { get; private set; }

    // Wind
    public double WindSpeed { get; private set; }
    public int WindDeg { get; private set; }

    public int Cloudiness { get; private set; }
    public double? Rain { get; private set; }
    public double? Snow { get; private set; }

    public DateTime CapturedAt { get; private set; }

    public WeatherReading(
        Guid stationId,
        string mainCondition, string description, string icon,
        double temp, double feelsLike, double minTemp, double maxTemp,
        int pressure, int humidity, int seaLevel, int groundLevel, int visibility,
        double windSpeed, int windDeg,
        int cloudiness, double? rain, double? snow,
        DateTime capturedAt)
    {
        if (stationId == Guid.Empty) throw new ArgumentException("Reading must belong to a station.");
        if (string.IsNullOrWhiteSpace(mainCondition)) throw new ArgumentException("Main condition required.");

        if (humidity < 0 || humidity > 100) throw new ArgumentException("Humidity must be 0-100.");
        if (pressure < 800 || pressure > 1100) throw new ArgumentException("Pressure seems unrealistic (check units)."); 

        StationId = stationId;
        MainCondition = mainCondition;
        Description = description;
        Icon = icon;

        Temperature = temp;
        FeelsLike = feelsLike;
        MinTemp = minTemp;
        MaxTemp = maxTemp;
        
        Pressure = pressure;
        Humidity = humidity;
        SeaLevel = seaLevel;
        GroundLevel = groundLevel;
        Visibility = visibility;

        WindSpeed = windSpeed;
        WindDeg = windDeg;

        Cloudiness = cloudiness;
        Rain = rain;
        Snow = snow;

        CapturedAt = capturedAt;
    }

    // EF Core Constructor
    protected WeatherReading() { }
}