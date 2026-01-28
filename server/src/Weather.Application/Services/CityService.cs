using System.Globalization;
using System.Linq.Expressions;
using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using Weather.Infrastructure.Mappers;

namespace Weather.Application.Services;

public class CityService : GenericService<City>, ICityService
{
    public CityService(IGenericRepository<City> repo) : base(repo) { }

    public async Task<List<CityDto>> QueryAsync(CityQuery? query = null)
    {
        query ??= new CityQuery();

        Expression<Func<City, bool>> predicate = c => true;

        // Filter by name
        if (!string.IsNullOrWhiteSpace(query.Name))
            predicate = Combine(predicate, c => c.Name.ToLower().Contains(query.Name.ToLower()));

        // Filter by country
        if (query.CountryId.HasValue)
            predicate = Combine(predicate, c => c.CountryId == query.CountryId.Value);

        // LookInsideBounds: filter by a small bounding box around lat/lng
        if (query.LookInsideBounds == true)
        {
            if (!string.IsNullOrWhiteSpace(query.Latitude) && double.TryParse(query.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var centerLat) &&
                !string.IsNullOrWhiteSpace(query.Longitude) && double.TryParse(query.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var centerLng))
            {
                double range = 0.5;
                double minLat = centerLat - range;
                double maxLat = centerLat + range;
                double minLng = centerLng - range;
                double maxLng = centerLng + range;

                predicate = Combine(predicate, c => c.Latitude >= minLat && c.Latitude <= maxLat && c.Longitude >= minLng && c.Longitude <= maxLng);
            }
        }
        else
        {
            // Filter by exact lat/lng if provided
            if (!string.IsNullOrWhiteSpace(query.Latitude) && double.TryParse(query.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat))
                predicate = Combine(predicate, c => c.Latitude == lat);

            if (!string.IsNullOrWhiteSpace(query.Longitude) && double.TryParse(query.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var lng))
                predicate = Combine(predicate, c => c.Longitude == lng);
        }

        var all = await _repo.FindAsync(predicate);

        // Pagination
        int skip = query.Page * query.PageSize;
        return all.Skip(skip).Take(query.PageSize).Select(c => c.ToDto()).ToList();
    }

    public async Task<CityDto?> FindOneDtoAsync(Expression<Func<City, bool>> predicate)
    {
        var result = await base.FindOneAsync(predicate);
        return result?.ToDto();
    }

    public async Task<CityDto> CreateCityAsync(CreateCityRequest request)
    {
        var newCity = new City(
            request.CountryId,
            request.Name,
            request.Latitude,
            request.Longitude,
            request.Timezone,
            request.Population
        );

        await CreateAsync(newCity);
        return newCity.ToDto();
    }

    public async Task<CityDto> UpdateCityAsync(Guid id, UpdateCityRequest request)
    {
        var city = await FindOneAsync(x => x.Id == id);
        if (city == null)
        {
            throw new KeyNotFoundException($"City with ID {id} not found.");
        }

        city.UpdateDetails(request.Name, request.Population, request.Timezone);
        await UpdateAsync(city);
        return city.ToDto();
    }

    public async Task DeleteCityAsync(Guid id)
    {
        var city = await FindOneAsync(x => x.Id == id);
        if (city == null)
        {
            throw new KeyNotFoundException($"City with ID {id} not found.");
        }

        await DeleteAsync(city);
    }
}
