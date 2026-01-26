using Microsoft.AspNetCore.Mvc;
using Weather.Application.Common.DTOs;
using Weather.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Weather.Application.Common.Models;

namespace Weather.API.Controllers;

[Authorize]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register", Name = "RegisterUser")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<UserRegisterResponseDto>>> Register([FromBody] UserRegisterRequestDto request)
    {
        var result = await _userService.RegisterAsync(request);
        return Ok(ApiResponse<UserRegisterResponseDto>.SuccessResponse(result, "Registration successful"));
    }

    [HttpPost("login", Name = "LoginUser")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<UserLoginResponseDto>>> Login([FromBody] UserLoginRequestDto request)
    {
        var token = await _userService.LoginAsync(request);
        return Ok(ApiResponse<UserLoginResponseDto>.SuccessResponse(token, "Login successful"));
    }

    [HttpGet("me", Name = "GetCurrentUser")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetMe()
    {
        var user = await _userService.GetMeAsync();
        return Ok(ApiResponse<UserDto>.SuccessResponse(user));
    }

    [HttpPatch("profile")]
    public async Task<ActionResult<ApiResponse<UserDto>>> EditProfile([FromBody] UserUpdateRequestDto request)
    {
        var updatedUser = await _userService.UpdateProfileAsync(request);
        return Ok(ApiResponse<UserDto>.SuccessResponse(updatedUser, "Profile updated"));
    }

    [HttpGet("{Id}", Name = "GetUserById")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUser(Guid Id)
    {
        var user = await _userService.GetByIdAsync(Id);
        if (user == null) return NotFound(ApiResponse<UserDto>.FailureResponse("User not found"));
        return Ok(ApiResponse<UserDto>.SuccessResponse(user));
    }

    [HttpGet(Name = "GetUsers")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<List<UserDto>>>> Users([FromQuery] UserQuery? query = null)
    {
        var users = await _userService.GetAllAsync(query);
        return Ok(ApiResponse<List<UserDto>>.SuccessResponse(users));
    }
}