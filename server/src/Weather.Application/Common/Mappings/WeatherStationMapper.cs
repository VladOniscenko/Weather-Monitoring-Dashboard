namespace Weather.Infrastructure.Mappers;
using Weather.Application.Common.DTOs;
using Weather.Domain.Entities;

public static class WeatherStationMapper
{
    public static WeatherStationDto ToDto(this WeatherStation ws) => new (
        ws.Id,
        ws.CityId,
        ws.Name,
        ws.Latitude,
        ws.Longitude,
        ws.LastSyncedAt
    );
}