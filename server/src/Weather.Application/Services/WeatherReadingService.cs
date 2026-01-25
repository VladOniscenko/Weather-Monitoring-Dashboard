using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;

namespace Weather.Application.Services;

public class WeatherReadingService : GenericService<WeatherReading>
{
    public WeatherReadingService(IWeatherReadingRepository repo) : base(repo) { }
}