using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using System.Linq.Expressions;
using Weather.Infrastructure.Mappers;

namespace Weather.Application.Services;

public class WeatherReadingService : GenericService<WeatherReading>, IWeatherReadingService
{
    private readonly ICurrentUserService _currentUser;
    private readonly IWeatherStationService _stationRepo;

    public WeatherReadingService(IWeatherReadingRepository repo, ICurrentUserService currentUser, IWeatherStationService sr) : base(repo)
    {
        _currentUser = currentUser;
        _stationRepo = sr;
    }

    public async Task<WeatherReadingDto> CreateAsync(CreateWeatherReadingRequest request)
    {
        var station = await _stationRepo.FindOneAsync(s => s.Id == request.StationId);
        if (station == null)
            throw new KeyNotFoundException("Station not found");

        if(station.UserId != _currentUser.Id)
            throw new UnauthorizedAccessException("Not allowed");
        
        var reading = new WeatherReading(
            request.StationId,
            request.MainCondition,
            request.Description,
            request.Icon,
            request.Temperature,
            request.FeelsLike,
            request.MinTemp,
            request.MaxTemp,
            request.Pressure,
            request.Humidity,
            request.SeaLevel,
            request.GroundLevel,
            request.Visibility,
            request.WindSpeed,
            request.WindDeg,
            request.Cloudiness,
            request.Rain,
            request.Snow,
            request.CapturedAt
        );

        await _repo.AddAsync(reading);
        return reading.ToDto();
    }

    public async Task DeleteAsync(Guid id)
    {
        var options = new FindOptions<WeatherReading>();
        options.Includes.Add(x => x.Station);

        var reading = await _repo.FindOneAsync(x => x.Id == id, options)
            ?? throw new KeyNotFoundException("WeatherReading not found");

        if (reading.Station.UserId != _currentUser.Id)
            throw new UnauthorizedAccessException("Not allowed");

        await _repo.DeleteAsync(reading);
    }
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