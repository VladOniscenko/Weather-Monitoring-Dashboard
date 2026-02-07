using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using Weather.Infrastructure.Mappers;
using System.Linq.Expressions;
namespace Weather.Application.Services;

public class WeatherStationService : GenericService<WeatherStation>, IWeatherStationService
{
    private readonly IWeatherStationRepository _stationRepo;
    private readonly ICurrentUserService _currentUser;

    public WeatherStationService(IWeatherStationRepository repo, ICurrentUserService currentUser) : base(repo)
    {
        _stationRepo = repo;
        _currentUser = currentUser;
    }

    public async Task<PagedResponse<WeatherStationDto>> QueryAsync(StationQuery? query = null)
    {
        return await QueryPagedAsync(query);
    }

    public async Task<PagedResponse<StationCordinateDto>> GetStationCordinatesAsync(StationQuery? query = null)
    {
        // Fetch paged stations (DTOs) from service
        var pagedStations = await QueryPagedAsync(query);

        // Map to coordinates while preserving pagination info
        var pagedCoordinates = new PagedResponse<StationCordinateDto>(
            pagedStations.Items.Select(x => new StationCordinateDto(
                x.Id,
                x.Latitude,
                x.Longitude
            )).ToList(),
            currentPage: pagedStations.CurrentPage,
            totalPages: pagedStations.TotalPages,
            totalItems: pagedStations.TotalItems
        );

        return pagedCoordinates;
    }

    public async Task<WeatherStationDto?> FindOneDtoAsync(Expression<Func<WeatherStation, bool>> predicate)
    {
        var result = await base.FindOneAsync(predicate);
        return result?.ToDto();
    }

    public async Task<Guid> CreateAsync(CreateWeatherStationRequest request)
    {
        var newWeatherStation = new WeatherStation(
            request.Name,
            request.Latitude,
            request.Longitude,
            request.CityId,
            _currentUser.Id
        );

        await _stationRepo.AddAsync(newWeatherStation);
        return newWeatherStation.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateWeatherStationRequest request)
    {
        var station = await FindOneAsync(x => x.Id == id) ?? throw new KeyNotFoundException("WeatherStation not found");

        if (station.UserId != _currentUser.Id)
            throw new UnauthorizedAccessException("Not allowed");

        station.UpdateDetails(
            request.Name,
            request.Latitude,
            request.Longitude,
            request.CityId
        );

        await _stationRepo.UpdateAsync(station);
        return true;
    }

    public async Task DeleteAsync(Guid id)
    {
        var station = await FindOneAsync(x => x.Id == id) ?? throw new KeyNotFoundException("WeatherStation not found");
        if (station.UserId != _currentUser.Id)
            throw new UnauthorizedAccessException("Not allowed");

        await _stationRepo.DeleteAsync(station);
    }

    public async Task<PagedResponse<WeatherStationDto>> QueryPagedAsync(StationQuery? query = null)
    {
        query ??= new StationQuery();

        // 1. Build the base predicate (filter)
        Expression<Func<WeatherStation, bool>> predicate = ws => true;

        // --- USER FILTER ---
        if (query.UserId.HasValue)
            predicate = Combine(predicate, ws => ws.UserId == query.UserId);

        // --- LATITUDE FILTER ---
        if (query.MinLat.HasValue && query.MaxLat.HasValue)
        {
            predicate = Combine(predicate, ws => ws.Latitude >= query.MinLat.Value && ws.Latitude <= query.MaxLat.Value);
        }

        // --- LONGITUDE FILTER (handles International Date Line)
        if (query.MinLng.HasValue && query.MaxLng.HasValue)
        {
            if (query.MinLng <= query.MaxLng)
                predicate = Combine(predicate, ws => ws.Longitude >= query.MinLng.Value && ws.Longitude <= query.MaxLng.Value);
            else
                predicate = Combine(predicate, ws => ws.Longitude >= query.MinLng.Value || ws.Longitude <= query.MaxLng.Value);
        }

        // --- CITY FILTER ---
        if (query.CityId.HasValue)
            predicate = Combine(predicate, ws => ws.CityId == query.CityId.Value);

        // --- NAME SEARCH FILTER ---
        if (!string.IsNullOrWhiteSpace(query.Name))
            predicate = Combine(predicate, ws => ws.Name.Contains(query.Name));

        // --- Count total matching stations ---
        var totalStations = await _repo.CountAsync(predicate);

        // --- Calculate paging ---
        var page = query.Page;
        var pageSize = query.PageSize;
        var totalPages = (int)Math.Ceiling((double)totalStations / pageSize);

        // --- Fetch paged data ---
        var options = new FindOptions<WeatherStation>
        {
            Page = page,
            Take = pageSize,
            IsAsNoTracking = true,
            IsIgnoreAutoIncludes = false
        };

        var stations = await _repo.FindAsync(predicate, options);
        var dtos = stations.Select(x => x.ToDto()).ToList();

        // --- Return paged response ---
        return new PagedResponse<WeatherStationDto>(
            dtos,
            currentPage: page,
            totalPages: totalPages,
            totalItems: totalStations
        );
    }
}
