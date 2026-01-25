using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;

namespace Weather.Application.Services;

public class WeatherStationService : GenericService<WeatherStation>, IWeatherStationService
{
    public WeatherStationService(IWeatherStationRepository repo) : base(repo) { }
}