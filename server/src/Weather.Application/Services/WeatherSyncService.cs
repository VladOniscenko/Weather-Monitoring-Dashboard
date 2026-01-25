using Weather.Application.Common.Interfaces;
using Weather.Domain.Entities;

public class WeatherSyncService : IWeatherSyncService
{
    private readonly IGenericRepository<WeatherStation> _stationRepo;
    private readonly IGenericRepository<WeatherReading> _readingRepo;
    private readonly IExternalWeatherApi _externalApi;

    public WeatherSyncService(
        IGenericRepository<WeatherStation> stationRepo,
        IGenericRepository<WeatherReading> readingRepo,
        IExternalWeatherApi externalApi)
    {
        _readingRepo = readingRepo;
        _stationRepo = stationRepo;
        _externalApi = externalApi;
    }

    public async Task SyncAllStationsAsync()
    {
        // Get all stations
        var stations = await _stationRepo.FindAsync(
            predicate: x => x.City.Country.Region == "Europe",
            new FindOptions<WeatherStation> { 
                Take = 50,
                OrderBy = q => q.OrderBy(x => x.LastSyncedAt)
            }
        );

        foreach (var station in stations)
        {
            // get weather data for station
            var data = await _externalApi.GetWeatherForCoordinates(station.Latitude, station.Longitude);

            var alreadyExists = await _readingRepo.AnyAsync(r =>
                r.StationId == station.Id &&
                r.CapturedAt == data.CapturedAt);

            if(alreadyExists)
                continue;

            var reading = new WeatherReading(
                station.Id,
                data.MainCondition, data.Description, data.Icon,
                data.Temp, data.FeelsLike, data.MinTemp, data.MaxTemp,
                data.Pressure, data.Humidity, data.SeaLevel, data.GroundLevel, data.Visibility,
                data.WindSpeed, data.WindDeg,
                data.Cloudiness, data.Rain, data.Snow,
                data.CapturedAt
            );

            await _readingRepo.AddAsync(reading);

            station.SyncedStation();
            await _stationRepo.UpdateAsync(station);
        }
    }
}