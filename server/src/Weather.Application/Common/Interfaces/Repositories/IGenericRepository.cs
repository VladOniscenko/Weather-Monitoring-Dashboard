using System.Linq.Expressions;
using Weather.Domain.Entities;

namespace Weather.Application.Common.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync(FindOptions<TEntity>? findOptions = null);

    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, FindOptions<TEntity>? findOptions = null);
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, FindOptions<TEntity>? findOptions = null);

    Task AddAsync(TEntity entity);
    Task AddManyAsync(IEnumerable<TEntity> entities);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);
    Task DeleteManyAsync(Expression<Func<TEntity, bool>> predicate);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
}
