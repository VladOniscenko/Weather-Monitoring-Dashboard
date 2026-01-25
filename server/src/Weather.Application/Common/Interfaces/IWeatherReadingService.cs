using Weather.Domain.Entities;
using Weather.Application.Common.DTOs;
using System.Linq.Expressions;
namespace Weather.Application.Common.Interfaces;

public interface IWeatherReadingService : IGenericService<WeatherReading>
{
    public Task<List<WeatherReadingDto>> QueryAsync(ReadingQuery? query = null);
    public Task<WeatherReadingDto?> FindOneDtoAsync(Expression<Func<WeatherReading, bool>> predicate);
}