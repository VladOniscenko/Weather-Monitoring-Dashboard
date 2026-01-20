
namespace Weather.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class WeatherReading : BaseEntity
{
    [ForeignKey("StationId")]
    public Guid StationId { get; set; }
    public WeatherStation Station { get; set; } = null!;

    // Weather
    public string MainCondition { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;

    // Main stats
    public double Temperature { get; set; }
    public double FeelsLike { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
    public int SeaLevel { get; set; }
    public int GroundLevel { get; set; }
    public int Visibility { get; set; }

    // Wind
    public double WindSpeed { get; set; }
    public int WindDeg { get; set; }

    // Clouds & Rain
    public int Cloudiness { get; set; }
    public double? Rain { get; set; }
    public double? Snow { get; set; }

    public DateTime CapturedAt { get; set; }
}