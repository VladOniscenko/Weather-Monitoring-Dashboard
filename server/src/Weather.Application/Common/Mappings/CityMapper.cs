namespace Weather.Infrastructure.Mappers;
using Weather.Application.Common.DTOs;
using Weather.Domain.Entities;

public static class CityMapper
{
    public static CityDto ToDto(this City city) => new (
        city.Id,
        city.CountryId,
        city.Name,
        city.Latitude,
        city.Longitude,
        city.Population,
        city.Timezone
    );
}