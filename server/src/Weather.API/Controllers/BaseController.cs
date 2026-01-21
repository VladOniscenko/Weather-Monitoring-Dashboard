using Microsoft.AspNetCore.Mvc;
using Weather.Application.Common.Models;

namespace Weather.API.Controllers;


public abstract class BaseController : ControllerBase
{
    protected Guid CurrentUserId
    {
        get
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out var userId) ? userId : Guid.Empty;
        }
    }

    protected IActionResult OkResponse<T>(T data, string? message = null)
    {
        return Ok(ApiResponse<T>.SuccessResponse(data, message));
    }

    protected IActionResult BadRequestResponse(string message, List<string>? errors = null)
    {
        return BadRequest(ApiResponse<object>.FailureResponse(message, errors));
    }

    protected IActionResult NotFoundResponse(string message)
    {
        return NotFound(ApiResponse<object>.FailureResponse(message));
    }
}