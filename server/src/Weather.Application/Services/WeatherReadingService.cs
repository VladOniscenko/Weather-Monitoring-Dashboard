using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using System.Linq.Expressions;
using Weather.Infrastructure.Mappers;

namespace Weather.Application.Services;

public class WeatherReadingService : GenericService<WeatherReading>, IWeatherReadingService
{
    public WeatherReadingService(IWeatherReadingRepository repo) : base(repo) { }

    public async Task<List<WeatherReadingDto>> QueryAsync(ReadingQuery? query = null)
    {
        query ??= new ReadingQuery();

        Expression<Func<WeatherReading, bool>> predicate = ws => true;

        if (query.StationId.HasValue)
            predicate = Combine(predicate, ws => ws.StationId == query.StationId.Value);

        if (query.Start.HasValue && !query.End.HasValue)
        {
            var dayStart = query.Start.Value.Date;
            var dayEnd = dayStart.AddDays(1);

            predicate = Combine(predicate,
                ws => ws.CreatedAt >= dayStart && ws.CreatedAt < dayEnd);
        }
        else if (query.Start.HasValue && query.End.HasValue)
        {
            var start = query.Start.Value;
            var end = query.End.Value;

            predicate = Combine(predicate,
                ws => ws.CreatedAt >= start && ws.CreatedAt <= end);
        }

        var results = await _repo.FindAsync(
            predicate,
            new FindOptions<WeatherReading> { Page = query.Page, Take = query.PageSize }
        );

        return results.Select(x => x.ToDto()).ToList();
    }

    public async Task<WeatherReadingDto?> FindOneDtoAsync(Expression<Func<WeatherReading, bool>> predicate)
    {
        var result = await base.FindOneAsync(predicate);
        return result?.ToDto();
    }
}