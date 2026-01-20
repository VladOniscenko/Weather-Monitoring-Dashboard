using Microsoft.AspNetCore.Mvc;
using Weather.Domain.DTOs;
using Weather.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Weather.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
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
        try
        {
            var result = await _userService.RegisterAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginRequestDto request)
    {
        try
        {
            var token = await _userService.LoginAsync(request);
            return Ok(token);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(new { message = "An error occurred during login." });
        }
    }


    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        try
        {
            var user = await _userService.GetMeAsync();
            return Ok(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(new { message = "An error occurred during fetching user." });
        }
    }



    [HttpPatch("profile")]
    public async Task<IActionResult> EditProfile([FromBody] UserUpdateRequestDto request)
    {
        try
        {
            var updatedUser = await _userService.UpdateProfileAsync(request);
            return Ok(updatedUser);
        }
        catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }



    [HttpGet("{Id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUser(Guid Id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(Id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(new { message = "An error occurred during fetching user." });
        }
    }



    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Users([FromQuery] UserQuery? query = null)
    {
        try
        {
            var users = await _userService.GetAllAsync(query);
            return Ok(users);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest(new { message = "An error occurred during fetching users." });
        }
    }
}