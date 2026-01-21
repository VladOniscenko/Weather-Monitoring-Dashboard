namespace Weather.Application.Common.DTOs;

public class CityQuery
{
    public string? Name { get; init; }
    public string? Latitude { get; init; }
    public string? Longitude { get; init; }
    public bool? LookInsideBounds { get; init; } = false;
    public Guid? CountryId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = (value > 100) ? 100 : value;
    }
    private readonly int _pageSize = 20;
}

public record CreateCityRequest(
    Guid CountryId,
    string Name,
    double Latitude,
    double Longitude,
    long Population,
    string Timezone
);

public record UpdateCityRequest(
    string Name,
    long Population,
    string Timezone
);