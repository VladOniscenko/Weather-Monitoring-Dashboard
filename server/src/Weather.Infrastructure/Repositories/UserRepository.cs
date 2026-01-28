using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Weather.Infrastructure.Persistence;
using Weather.Application.Common.DTOs;

namespace Weather.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<List<User>> GetUsersAsync(UserQuery? query = null)
    {
        query ??= new UserQuery();
        var q = _context.Set<User>().AsQueryable();

        if (!string.IsNullOrEmpty(query.Email))
            q = q.Where(u => u.Email.Contains(query.Email));

        if (query.Role.HasValue)
            q = q.Where(u => u.Role == query.Role);

        if (query.IsActive.HasValue)
            q = q.Where(u => u.IsActive == query.IsActive);

        return await q
            .Skip(query.Page * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();
    }
}