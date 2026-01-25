namespace Weather.Application.Common.DTOs;

public class StationQuery
{
    public Guid? CityId { get; set; }
    public string? Name { get; set; }


    public int Page { get; init; } = 1;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = (value > 100) ? 100 : value;
    }
    private readonly int _pageSize = 20;
}