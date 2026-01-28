using Weather.Domain.Enums;

namespace Weather.Application.Common.DTOs;

public record UserRegisterRequestDto(
    string Name,
    string Email,
    string Password
);



public record UserRegisterResponseDto(
    string Name,
    string Email
);



public record UserLoginRequestDto(
    string Email,
    string Password
);



public record UserLoginResponseDto(
    string Token,
    DateTime ExpiresAt
);

public record UserDto(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt,
    UserRole Role
);



public class UserQuery
{
    public string? Email { get; init; }
    public UserRole? Role { get; init; }
    public bool? IsActive { get; init; }
    public int Page { get; init; } = 0;
    public int PageSize { get; init; } = 20;
}


public record UserUpdateRequestDto(
    string? Name,
    string? Email
);