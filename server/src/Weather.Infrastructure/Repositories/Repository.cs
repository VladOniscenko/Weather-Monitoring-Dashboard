using System.Linq.Expressions;
using Weather.Infrastructure.Persistence;
using Weather.Application.Common.Interfaces;
using Weather.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Weather.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{

    protected readonly AppDbContext _context;
    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }


    public IQueryable<TEntity> Queryable => _context.Set<TEntity>();
    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }



    public async Task AddManyAsync(IEnumerable<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }



    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().AnyAsync(predicate);
    }


    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return _context.Set<TEntity>().CountAsync(predicate);
    }



    public async Task DeleteAsync(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }



    public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = await FindAsync(predicate);
        _context.Set<TEntity>().RemoveRange(entities);
        await _context.SaveChangesAsync();
    }



    public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, FindOptions<TEntity>? findOptions = null)
    {
        return await Get(findOptions).FirstOrDefaultAsync(predicate);
    }



    public async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }



    public async Task<List<TEntity>> GetAllAsync(FindOptions<TEntity>? findOptions = null)
    {
        return await Get(findOptions).ToListAsync();
    }



    public async Task<List<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        FindOptions<TEntity>? findOptions = null)
    {
        // 1. Start with the raw set
        IQueryable<TEntity> query = _context.Set<TEntity>();

        // 2. Apply the Filter FIRST (Where)
        query = query.Where(predicate);

        // 3. Apply the 'FindOptions' (Order, Take, Skip) to the filtered result
        return await ApplyOptions(query, findOptions).ToListAsync();
    }


    // query builder
    private IQueryable<TEntity> Get(FindOptions<TEntity>? options = null)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();
        return ApplyOptions(query, options);
    }

    private IQueryable<TEntity> ApplyOptions(IQueryable<TEntity> query, FindOptions<TEntity>? options)
    {
        if (options == null)
        {
            return query;
        }

        // Order the FILTERED list
        if (options.OrderBy != null)
            query = options.OrderBy(query);

        // Page the FILTERED list
        if (options.Take > 0)
        {
            if (options.Page > 0)
                query = query.Skip(options.Page.Value * options.Take.Value);
            query = query.Take(options.Take.Value);
        }

        // EF CORE OPTIONS
        if (options.IsIgnoreAutoIncludes)
            query = query.IgnoreAutoIncludes();
        if (options.IsAsNoTracking)
            query = query.AsNoTracking();

        return query;
    }
}
