using Weather.Domain.Entities;
using Weather.Application.Common.DTOs;
using System.Linq.Expressions;

namespace Weather.Application.Common.Interfaces;

public interface IWeatherStationService : IGenericService<WeatherStation>
{
    public Task<List<WeatherStationDto>> QueryAsync(StationQuery? query = null);
    public Task<List<StationCordinateDto>> GetStationCordinatesAsync(StationQuery? query = null);
    public Task<WeatherStationDto?> FindOneDtoAsync(Expression<Func<WeatherStation, bool>> predicate);
}