using Weather.Domain.Entities;
using Weather.Application.Common.DTOs;

namespace Weather.Application.Common.Interfaces;

public interface IWeatherReadingService : IGenericService<WeatherReading>
{
    public Task<List<WeatherReading>> QueryAsync(ReadingQuery? query = null);
}