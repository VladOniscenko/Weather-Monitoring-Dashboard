using Weather.Domain.Entities;
using Weather.Application.Common.DTOs;

namespace Weather.Application.Common.Interfaces;

public interface ICountryService : IGenericService<Country>
{
    public Task<List<CountryDto>> GetAllDtosAsync();
    public Task<CountryDto?> FindOneDtoAsync(System.Linq.Expressions.Expression<System.Func<Country, bool>> predicate);
    public Task<CountryDto> CreateCountryAsync(CreateCountryRequest request);
    public Task<CountryDto> UpdateCountryAsync(Guid id, UpdateCountryRequest request);
    public Task DeleteCountryAsync(Guid id);
}