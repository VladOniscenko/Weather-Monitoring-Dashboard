namespace Weather.Infrastructure.Mappers;
using Weather.Application.Common.DTOs;
using Weather.Domain.Entities;

public static class CountryMapper
{
    public static CountryDto ToDto(this Country country) => new (
        country.Id,
        country.Name,
        country.CCA2,
        country.CCA3,
        country.Region,
        country.Subregion,
        country.Capital,
        country.Flag,
        country.Latitude,
        country.Longitude,
        country.Independent,
        country.Landlocked
    );
}