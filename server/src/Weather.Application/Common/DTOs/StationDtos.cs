namespace Weather.Application.Common.DTOs;

public class StationQuery
{
    public Guid? CityId { get; set; }
    public string? Name { get; set; }

    public double? MinLng { get; set; }
    public double? MaxLng { get; set; }
    public double? MinLat { get; set; }
    public double? MaxLat { get; set; }
    public int? Zoom { get; set; }

    public int Page { get; init; } = 0;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = (value > 1000) ? 1000 : value;
    }
    private readonly int _pageSize = 1000;
}