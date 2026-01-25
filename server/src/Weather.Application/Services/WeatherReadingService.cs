using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using System.Linq.Expressions;

namespace Weather.Application.Services;

public class WeatherReadingService : GenericService<WeatherReading>, IWeatherReadingService
{
    public WeatherReadingService(IWeatherReadingRepository repo) : base(repo) { }

    public async Task<List<WeatherReading>> QueryAsync(ReadingQuery? query = null)
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


        return await _repo.FindAsync(
            predicate,
            new FindOptions<WeatherReading> { Page = query.Page - 1, Take = query.PageSize }
        );
    }
}