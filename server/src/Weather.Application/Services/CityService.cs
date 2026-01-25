using System.Globalization;
using System.Linq.Expressions;
using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;

namespace Weather.Application.Services;

public class CityService : GenericService<City>
{
    public CityService(IGenericRepository<City> repo) : base(repo) { }

    public async Task<List<City>> QueryAsync(CityQuery? query = null)
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
        int skip = (query.Page - 1) * query.PageSize;
        return all.Skip(skip).Take(query.PageSize).ToList();
    }
}
