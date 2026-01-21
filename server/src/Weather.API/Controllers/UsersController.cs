using Microsoft.AspNetCore.Mvc;
using Weather.Application.Common.DTOs;
using Weather.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Weather.API.Controllers;

[Authorize]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto request)
    {
        var result = await _userService.RegisterAsync(request);
        return OkResponse(result, "Registration successful");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto request)
    {
        var token = await _userService.LoginAsync(request);
        return OkResponse(token, "Login successful");
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var user = await _userService.GetMeAsync();
        return OkResponse(user);
    }

    [HttpPatch("profile")]
    public async Task<IActionResult> EditProfile([FromBody] UserUpdateRequestDto request)
    {
        var updatedUser = await _userService.UpdateProfileAsync(request);
        return OkResponse(updatedUser, "Profile updated");
    }

    [HttpGet("{Id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUser(Guid Id)
    {
        var user = await _userService.GetByIdAsync(Id);
        if (user == null) return NotFoundResponse("User not found");
        return OkResponse(user);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Users([FromQuery] UserQuery? query = null)
    {
        var users = await _userService.GetAllAsync(query);
        return OkResponse(users);
    }
}