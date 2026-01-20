namespace Weather.Domain.Entities;

public class FindOptions
{
    public bool IsIgnoreAutoIncludes { get; set; }
    public bool IsAsNoTracking { get; set; } = true;
}
