using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Domain.DTOs;
using Weather.Domain.Common.Validators;
using Weather.Infrastructure.Mappers;


namespace Weather.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _context;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }



    public async Task<UserRegisterResponseDto> RegisterAsync(UserRegisterRequestDto request)
    {
        // validate email
        if (!EmailValidator.IsValidEmail(request.Email))
        {
            throw new ArgumentException("Invalid email.");
        }

        // check if email not taken
        var exists = await _context.AnyAsync(u => u.Email == request.Email);
        if (exists) throw new Exception("User already exists.");

        // create user
        var user = new User
        {
            Email = request.Email,
            Name = request.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        // save user
        await _context.AddAsync(user);
        return new UserRegisterResponseDto(
            user.Name,
            user.Email
        );
    }



    public async Task<UserDto> UpdateProfileAsync(Guid userId, UserUpdateRequestDto request)
    {
        // Find user by its id
        var user = await _context.FindOneAsync(u => u.Id == userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

        // validate if name is not empty
        if (!string.IsNullOrEmpty(request.Name))
        {
            user.Name = request.Name;
        }

        // validate if email is not empty
        if (!string.IsNullOrEmpty(request.Email))
        {
            // validate email
            if (!EmailValidator.IsValidEmail(request.Email))
            {
                throw new ArgumentException("Invalid email.");
            }

            // check if email not taken
            var exists = await _context.AnyAsync(u => u.Email == request.Email);
            if (exists)
            {
                throw new Exception("Email already in use.");
            }

            user.Email = request.Email;
        }


        // save it
        await _context.UpdateAsync(user);
        return user.ToDto();
    }



    public async Task<UserLoginResponseDto> LoginAsync(UserLoginRequestDto request)
    {
        // Fetch user by email only
        var user = await _context.FindOneAsync(u => u.Email == request.Email);

        // Security best practice: If user doesn't exist OR is deleted, 
        // treat them as "Null" to prevent email harvesting.
        if (user == null || user.IsDeleted)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Check if account is active
        if (!user.IsActive || user.Name == "")
        {
            throw new UnauthorizedAccessException("Account is not active.");
        }

        // Finally, verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        var tokenData = _tokenService.CreateToken(user);
        return new UserLoginResponseDto(
            tokenData["token"],
            DateTime.Parse(tokenData["expiresAt"])
        );
    }



    public async Task<UserDto> GetByIdAsync(Guid userId)
    {
        // get user by id
        var user = await _context.FindOneAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {userId} not found.");
        }

        return user.ToDto();
    }



    public async Task<List<UserDto>> GetAllAsync(UserQuery? query = null)
    {
        var users = await _context.GetUsersAsync(query);
        return users.Select(user => user.ToDto()).ToList();
    }
}