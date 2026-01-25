namespace Weather.Application.Common.DTOs;

public class ReadingQuery
{
    public DateTime? Start { get; set; } = DateTime.UtcNow;
    public DateTime? End { get; set; }

    public Guid? StationId { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = (value > 100) ? 100 : value;
    }
    private readonly int _pageSize = 20;
}