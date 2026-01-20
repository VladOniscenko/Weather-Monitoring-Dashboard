using Weather.Domain.Entities;
using Weather.Domain.DTOs;

namespace Weather.Application.Common.Interfaces;


public interface IUserRepository : IGenericRepository<User>
{
    public Task<List<User>> GetUsersAsync(UserQuery? query = null);
}