using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using Weather.Application.Common.Models;

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

    [HttpGet(Name = "GetReadings")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<WeatherReadingDto>>>> GetAll([FromQuery] ReadingQuery? query = null)
    {
        var cities = await _service.QueryAsync(query);
        return Ok(ApiResponse<List<WeatherReadingDto>>.SuccessResponse(cities));
    }

    [HttpGet("{id}", Name = "GetReadingById")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<WeatherReadingDto>>> GetById(Guid id)
    {
        var city = await _service.FindOneDtoAsync(x => x.Id == id);
        if (city == null)
            return NotFound(ApiResponse<WeatherReadingDto>.FailureResponse("Reading not found"));

        return Ok(ApiResponse<WeatherReadingDto>.SuccessResponse(city));
    }
}