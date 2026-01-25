using Weather.Domain.Entities;
using Weather.Application.Common.DTOs;

namespace Weather.Application.Common.Interfaces;

public interface IWeatherStationService : IGenericService<WeatherStation>
{
    public Task<List<WeatherStation>> QueryAsync(StationQuery? query = null);
}