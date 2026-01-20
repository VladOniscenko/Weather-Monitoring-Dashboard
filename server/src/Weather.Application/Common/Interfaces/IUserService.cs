using Weather.Domain.DTOs;

namespace Weather.Application.Common.Interfaces;

public interface IUserService
{
    Task<UserRegisterResponseDto> RegisterAsync(UserRegisterRequestDto request);
    Task<UserLoginResponseDto> LoginAsync(UserLoginRequestDto request);
    Task<UserDto> GetByIdAsync(Guid userId);
    Task<List<UserDto>> GetAllAsync(UserQuery? query = null);
    Task<UserDto> UpdateProfileAsync(Guid userId, UserUpdateRequestDto request);
}