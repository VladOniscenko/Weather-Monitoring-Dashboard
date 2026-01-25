namespace Weather.Infrastructure.Mappers;
using Weather.Application.Common.DTOs;
using Weather.Domain.Entities;


public static class WeatherReadingMapper
{
    public static WeatherReadingDto ToDto(this WeatherReading wr) => new (
        wr.Id,
        wr.StationId,
        wr.MainCondition,
        wr.Description,
        wr.Icon,
        wr.Temperature,
        wr.FeelsLike,
        wr.MinTemp,
        wr.MaxTemp,
        wr.Pressure,
        wr.Humidity,
        wr.SeaLevel,
        wr.GroundLevel,
        wr.Visibility,
        wr.WindSpeed,
        wr.WindDeg,
        wr.Cloudiness,
        wr.Rain,
        wr.Snow,
        wr.CapturedAt
    );
}