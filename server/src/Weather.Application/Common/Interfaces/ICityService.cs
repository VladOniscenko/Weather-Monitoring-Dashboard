using Weather.Domain.Entities;
using Weather.Application.Common.DTOs;
using System.Linq.Expressions;

namespace Weather.Application.Common.Interfaces;

public interface ICityService : IGenericService<City>
{
    public Task<List<CityDto>> QueryAsync(CityQuery? query = null);
    public Task<CityDto?> FindOneDtoAsync(Expression<Func<City, bool>> predicate);
    public Task<CityDto> CreateCityAsync(CreateCityRequest request);
    public Task<CityDto> UpdateCityAsync(Guid id, UpdateCityRequest request);
    public Task DeleteCityAsync(Guid id);
}