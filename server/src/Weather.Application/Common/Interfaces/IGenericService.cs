using System.Linq.Expressions;

namespace Weather.Application.Common.Interfaces;

public interface IGenericService<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate);

    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);

    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
}
    