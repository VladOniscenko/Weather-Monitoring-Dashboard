namespace Weather.Domain.Entities;

public class FindOptions<TEntity>
{
    public bool IsIgnoreAutoIncludes { get; set; }
    public bool IsAsNoTracking { get; set; } = true;
    public int? Take { get; set; } = null;
    public int? Page { get; set; } = null;
    public Func<IQueryable<TEntity>, IQueryable<TEntity>>? OrderBy { get; set; }
}
