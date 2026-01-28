using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using Weather.Infrastructure.Mappers;
using System.Linq.Expressions;
using System.Globalization;

namespace Weather.Application.Services;

public class CountryService : GenericService<Country>, ICountryService
{
    public CountryService(IGenericRepository<Country> repo) : base(repo) { }

    public async Task<List<CountryDto>> GetAllDtosAsync()
    {
        var countries = await GetAllAsync();
        return countries.Select(c => c.ToDto()).ToList();
    }

    public async Task<CountryDto?> FindOneDtoAsync(System.Linq.Expressions.Expression<System.Func<Country, bool>> predicate)
    {
        var result = await base.FindOneAsync(predicate);
        return result?.ToDto();
    }

    public async Task<CountryDto> CreateCountryAsync(CreateCountryRequest request)
    {
        var newCountry = new Country(
            request.Name,
            request.CCA2,
            request.CCA3,
            request.Region,
            request.Subregion,
            request.Capital,
            request.Flag,
            request.Latitude,
            request.Longitude,
            request.Independent,
            request.Landlocked
        );

        await CreateAsync(newCountry);
        return newCountry.ToDto();
    }

    public async Task<CountryDto> UpdateCountryAsync(Guid id, UpdateCountryRequest request)
    {
        var country = await FindOneAsync(x => x.Id == id);
        if (country == null)
        {
            throw new KeyNotFoundException($"Country with ID {id} not found.");
        }

        country.UpdateDetails(
            request.Name,
            request.CCA2,
            request.CCA3,
            request.Region,
            request.Subregion,
            request.Capital,
            request.Flag,
            request.Latitude,
            request.Longitude,
            request.Independent,
            request.Landlocked
        );

        await UpdateAsync(country);
        return country.ToDto();
    }

    public async Task DeleteCountryAsync(Guid id)
    {
        var country = await FindOneAsync(x => x.Id == id);
        if (country == null)
        {
            throw new KeyNotFoundException($"Country with ID {id} not found.");
        }

        await DeleteAsync(country);
    }

    public async Task<List<CountryDto>> QueryAsync(CountryQuery? query = null)
    {
        query ??= new CountryQuery();
        Expression<Func<Country, bool>> predicate = c => true;

        // Filter by Name
        if (!string.IsNullOrWhiteSpace(query.Name))
            predicate = Combine(predicate, c => c.Name.ToLower().Contains(query.Name.ToLower()));
        // Filter by CCA2
        if (!string.IsNullOrWhiteSpace(query.CCA2))
            predicate = Combine(predicate, c => c.CCA2.ToLower().Contains(query.CCA2.ToLower()));
        // Filter by CCA3
        if (!string.IsNullOrWhiteSpace(query.CCA3))
            predicate = Combine(predicate, c => c.CCA3.ToLower().Contains(query.CCA3.ToLower()));
        // Filter by Region
        if (!string.IsNullOrWhiteSpace(query.Region))
            predicate = Combine(predicate, c => c.Region.ToLower().Contains(query.Region.ToLower()));
        // Filter by Subregion
        if (!string.IsNullOrWhiteSpace(query.Subregion))
            predicate = Combine(predicate, c => c.Subregion.ToLower().Contains(query.Subregion.ToLower()));
        // Filter by Capital
        if (!string.IsNullOrWhiteSpace(query.Capital))
            predicate = Combine(predicate, c => c.Capital.ToLower().Contains(query.Capital.ToLower()));

        // Filter by Independent
        if (query.Independent == true || query.Independent == false)
            predicate = Combine(predicate, c => c.Independent == query.Independent);
        // Filter by Landlocked
        if (query.Landlocked == true || query.Landlocked == false)
            predicate = Combine(predicate, c => c.Landlocked == query.Landlocked);

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
}