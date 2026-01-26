using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using Weather.Infrastructure.Mappers;

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
}