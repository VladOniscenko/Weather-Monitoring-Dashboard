using Weather.Domain.Entities;
using Weather.Application.Common.DTOs;
using System.Linq.Expressions;

namespace Weather.Application.Common.Interfaces;

public interface IWeatherStationService : IGenericService<WeatherStation>
{
    public Task<PagedResponse<WeatherStationDto>> QueryAsync(StationQuery? query = null);
    public Task<PagedResponse<StationCordinateDto>> GetStationCordinatesAsync(StationQuery? query = null);
    public Task<WeatherStationDto?> FindOneDtoAsync(Expression<Func<WeatherStation, bool>> predicate);
    public Task<Guid> CreateAsync(CreateWeatherStationRequest request);
    public Task<bool> UpdateAsync(Guid id, UpdateWeatherStationRequest request);
    public Task DeleteAsync(Guid id);
    public Task<PagedResponse<WeatherStationDto>> QueryPagedAsync(StationQuery? query = null);
}