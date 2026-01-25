namespace Weather.Application.Common.DTOs;

public record WeatherReadingDto(
    Guid Id,
    Guid StationId,
    string MainCondition,
    string Description,
    string Icon,
    double Temperature,
    double FeelsLike,
    double MinTemp,
    double MaxTemp,
    int Pressure,
    int Humidity,
    int SeaLevel,
    int GroundLevel,
    int Visibility,
    double WindSpeed,
    double WindDeg,
    int Cloudiness,
    double? Rain,
    double? Snow,
    DateTime CapturedAt
);