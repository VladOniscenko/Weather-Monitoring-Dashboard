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
    private readonly ICurrentUserService _currentUser;

    public UserService(IUserRepository context, ITokenService tokenService, ICurrentUserService currentUser)
    {
        _context = context;
        _tokenService = tokenService;
        _currentUser = currentUser;
    }



    public async Task<UserRegisterResponseDto> RegisterAsync(UserRegisterRequestDto request)
    {
        // validate email
        if (!EmailValidator.IsValidEmail(request.Email))
            throw new ArgumentException("Invalid email.");

        // check if email not taken
        var exists = await _context.AnyAsync(u => u.Email == request.Email);
        if (exists) throw new Exception("User already exists.");

        // create user
        var user = new User(
            request.Email,
            request.Name,
            BCrypt.Net.BCrypt.HashPassword(request.Password)
        );

        // save user
        await _context.AddAsync(user);
        return new UserRegisterResponseDto(
            user.Name,
            user.Email
        );
    }



    public async Task<UserDto> UpdateProfileAsync(UserUpdateRequestDto request)
    {
        // Find user by its id
        var user = await _context.FindOneAsync(u => u.Id == _currentUser.Id)
            ?? throw new KeyNotFoundException($"User with ID {_currentUser.Id} not found.");

        string finalName = !string.IsNullOrEmpty(request.Name)
            ? request.Name
            : user.Name;

        string finalEmail = user.Email;

        // validate if email is not empty
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            // validate email
            if (!EmailValidator.IsValidEmail(request.Email))
                throw new ArgumentException("Invalid email.");

            var exists = await _context.AnyAsync(u => u.Email == request.Email);
            if (exists)
                throw new Exception("Email already in use.");

            finalEmail = request.Email;
        }
        
        user.UpdateDetails(finalName, finalEmail);

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



    public async Task<UserDto> GetMeAsync()
    {
        return await GetByIdAsync(_currentUser.Id);
    }



    public async Task<List<UserDto>> GetAllAsync(UserQuery? query = null)
    {
        var users = await _context.GetUsersAsync(query);
        return users.Select(user => user.ToDto()).ToList();
    }
}