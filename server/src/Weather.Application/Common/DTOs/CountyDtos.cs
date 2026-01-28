namespace Weather.Application.Common.DTOs;

public record CountryDto(
    Guid Id,
    string Name,
    string CCA2,
    string CCA3,
    string Region,
    string Subregion,
    string Capital,
    string Flag,
    double Latitude,
    double Longitude,
    bool Independent,
    bool Landlocked
);

public record CreateCountryRequest(
    string Name,
    string CCA2,
    string CCA3,
    string Region,
    string Subregion,
    string Capital,
    string Flag,
    double Latitude,
    double Longitude,
    bool Independent,
    bool Landlocked
);

public record UpdateCountryRequest(
    string Name,
    string CCA2,
    string CCA3,
    string Region,
    string Subregion,
    string Capital,
    string Flag,
    double Latitude,
    double Longitude,
    bool Independent,
    bool Landlocked
);

public class CountryQuery
{
    public string? Name { get; init; }
    public string? CCA2 { get; init; }
    public string? CCA3 { get; init; }
    public string? Region { get; init; }
    public string? Subregion { get; init; }
    public string? Capital { get; init; }
    public bool? Independent { get; init; }
    public bool? Landlocked { get; init; }
    public string? Latitude { get; init; }
    public string? Longitude { get; init; }
    public bool? LookInsideBounds { get; init; } = false;
    public int Page { get; init; } = 0;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value;
    }
    private readonly int _pageSize = 1000;
}