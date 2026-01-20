using System.Linq.Expressions;
using Weather.Application.Common.Interfaces;

namespace Weather.Application.Services;

public class GenericService<T> : IGenericService<T> where T : class
{
    protected readonly IGenericRepository<T> _repo;

    public GenericService(IGenericRepository<T> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repo.GetAllAsync();
    }

    public async Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate)
    {
        return await _repo.FindOneAsync(predicate);
    }

    public async Task CreateAsync(T entity)
    {
        await _repo.AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        await _repo.DeleteAsync(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _repo.AnyAsync(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _repo.CountAsync(predicate);
    }
}
