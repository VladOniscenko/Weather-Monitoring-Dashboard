using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using Weather.Infrastructure.Mappers;
using System.Linq.Expressions;
namespace Weather.Application.Services;

public class WeatherStationService : GenericService<WeatherStation>, IWeatherStationService
{
    public WeatherStationService(IWeatherStationRepository repo) : base(repo) { }

    public async Task<List<WeatherStationDto>> QueryAsync(StationQuery? query = null)
    {
        query ??= new StationQuery();

        Expression<Func<WeatherStation, bool>> predicate = ws => true;

        if (query.CityId.HasValue)
            predicate = Combine(predicate, ws => ws.CityId == query.CityId.Value);

        if (!string.IsNullOrWhiteSpace(query.Name))
            predicate = Combine(predicate, c => c.Name.ToLower().Contains(query.Name.ToLower()));

        return (await _repo.FindAsync(
            predicate,
            new FindOptions<WeatherStation> { Page = query.Page - 1, Take = query.PageSize }
        )).Select(x => x.ToDto()).ToList();
    }

    public async Task<WeatherStationDto?> FindOneDtoAsync(Expression<Func<WeatherStation, bool>> predicate)
    {
        var result = await base.FindOneAsync(predicate);
        return result?.ToDto();
    }
}