namespace Weather.Application.Common.Interfaces;

public record ExternalWeatherDto(
    string MainCondition, string Description, string Icon,
    double Temp, double FeelsLike, double MinTemp, double MaxTemp,
    int Pressure, int Humidity, int SeaLevel, int GroundLevel,
    int Visibility, double WindSpeed, int WindDeg,
    int Cloudiness, double? Rain, double? Snow,
    DateTime CapturedAt
);

public interface IExternalWeatherApi
{
    Task<ExternalWeatherDto> GetWeatherForCoordinates(double lat, double lon);
}