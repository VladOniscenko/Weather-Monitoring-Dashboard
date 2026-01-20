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



    public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
    {
        return await Get(findOptions).FirstOrDefaultAsync(predicate);
    }



    public async Task UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
    }



    public async Task<List<TEntity>> GetAllAsync(FindOptions? findOptions = null)
    {
        return await Get(findOptions).ToListAsync();
    }



    public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, FindOptions? findOptions = null)
    {
        return await Get(findOptions).Where(predicate).ToListAsync();
    }



    // query builder
    private IQueryable<TEntity> Get(FindOptions? findOptions = null)
    {
        findOptions ??= new FindOptions();
        var entity = _context.Set<TEntity>();

        if (findOptions.IsAsNoTracking && findOptions.IsIgnoreAutoIncludes)
            entity.IgnoreAutoIncludes().AsNoTracking();
        else if (findOptions.IsIgnoreAutoIncludes)
            entity.IgnoreAutoIncludes();
        else if (findOptions.IsAsNoTracking)
            entity.AsNoTracking();
        return entity;
    }
}
