using Microsoft.AspNetCore.Mvc;

namespace Weather.Api.Controllers;


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
}