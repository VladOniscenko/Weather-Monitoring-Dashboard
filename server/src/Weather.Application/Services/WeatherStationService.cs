using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using Weather.Infrastructure.Mappers;
using System.Linq.Expressions;
namespace Weather.Application.Services;

public class WeatherStationService : GenericService<WeatherStation>, IWeatherStationService
{
    private readonly IWeatherStationRepository _stationRepo;

    public WeatherStationService(IWeatherStationRepository repo) : base(repo)
    {
        _stationRepo = repo;
    }

    public async Task<List<WeatherStationDto>> QueryAsync(StationQuery? query = null)
    {
        query ??= new StationQuery();
        var stations = await GetStationsAsync(query);
        return stations.Select(x => x.ToDto()).ToList();
    }


    public async Task<List<StationCordinateDto>> GetStationCordinatesAsync(StationQuery? query = null)
    {
        query ??= new StationQuery();
        var stations = await GetStationsAsync(query);
        return stations.Select(x => new StationCordinateDto(
            x.Id,
            x.Latitude,
            x.Longitude
        )).ToList();
    }

    public async Task<WeatherStationDto?> FindOneDtoAsync(Expression<Func<WeatherStation, bool>> predicate)
    {
        var result = await base.FindOneAsync(predicate);
        return result?.ToDto();
    }

    private async Task<List<WeatherStation>> GetStationsAsync(StationQuery? query = null)
    {
        query ??= new StationQuery();

        // 1. Build the Predicate (Filter Logic)
        Expression<Func<WeatherStation, bool>> predicate = ws => true;

        // --- LATITUDE FILTER ---
        if (query.MinLat.HasValue && query.MaxLat.HasValue)
        {
            predicate = Combine(predicate, ws => ws.Latitude >= query.MinLat.Value && ws.Latitude <= query.MaxLat.Value);
        }

        // --- LONGITUDE FILTER (CRITICAL FOR PRECISION) ---
        // This handles the "World Wrapping" issue (International Date Line)
        if (query.MinLng.HasValue && query.MaxLng.HasValue)
        {
            if (query.MinLng <= query.MaxLng)
                predicate = Combine(predicate, ws => ws.Longitude >= query.MinLng.Value && ws.Longitude <= query.MaxLng.Value);
            else
                predicate = Combine(predicate, ws => ws.Longitude >= query.MinLng.Value || ws.Longitude <= query.MaxLng.Value);
        }

        // --- SEARCH FILTERS ---
        if (query.CityId.HasValue)
            predicate = Combine(predicate, ws => ws.CityId == query.CityId.Value);

        if (!string.IsNullOrWhiteSpace(query.Name))
            predicate = Combine(predicate, ws => ws.Name.Contains(query.Name));

        var options = new FindOptions<WeatherStation>
        {
            Page = query.Page,
            Take = query.PageSize,
            IsAsNoTracking = true,
            IsIgnoreAutoIncludes = false
        };

        // 2. Fetch Data
        var stations = await _repo.FindAsync(predicate, options);
        return stations;
    }
}
