namespace Weather.Application.Common.DTOs;

public record CreateWeatherStationRequest(
    string Name, 
    double Latitude,
    double Longitude,
    Guid CountryId
);

public record UpdateWeatherStationRequest(
    string Name, 
    double Latitude,
    double Longitude,
    Guid CountryId
);