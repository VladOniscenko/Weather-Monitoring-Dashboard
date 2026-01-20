namespace Weather.Infrastructure.Mappers;
using Weather.Domain.DTOs;
using Weather.Domain.Entities;

public static class UserMapper
{
    public static UserDto ToDto(this User user) => new (
        user.Id,
        user.Name,
        user.Email,
        user.CreatedAt,
        user.Role
    );
}