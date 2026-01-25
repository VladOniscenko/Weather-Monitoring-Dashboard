namespace Weather.Application.Common.DTOs;

public record CreateWeatherStationRequest(
    string Name,
    double Latitude,
    double Longitude,
    Guid CityId
);

public record UpdateWeatherStationRequest(
    string Name,
    double Latitude,
    double Longitude,
    Guid CityId
);

public record WeatherStationDto(
    Guid Id,
    Guid CityId,
    string Name,
    double Latitude,
    double Longitude,
    DateTime LastSyncedAt
);