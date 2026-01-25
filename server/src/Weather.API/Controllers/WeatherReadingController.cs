using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;

namespace Weather.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class WeatherReadingController : BaseController
{
    private readonly IWeatherReadingService _service;
    public WeatherReadingController(IWeatherReadingService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] ReadingQuery? query = null)
    {
        var cities = await _service.QueryAsync(query);
        return OkResponse(cities);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var city = await _service.FindOneAsync(x => x.Id == id);
        if (city == null)
            return NotFoundResponse("Reading not found");

        return OkResponse(city);
    }
}