using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using Weather.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Weather.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class WeatherStationsController : BaseController
{
    private readonly IWeatherStationService _service;

    public WeatherStationsController(IWeatherStationService service)
    {
        _service = service;
    }

    [HttpGet(Name = "GetAllStations")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<WeatherStationDto>>>> GetAll([FromQuery] StationQuery? query = null)
    {
        var stations = await _service.QueryAsync(query);
        return Ok(ApiResponse<List<WeatherStationDto>>.SuccessResponse(stations));
    }

    [HttpGet("/cordinates", Name = "GetAllStationsCordinates")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<StationCordinateDto>>>> GetAllCordinates([FromQuery] StationQuery? query = null)
    {
        var stations = await _service.GetStationCordinatesAsync(query);
        return Ok(ApiResponse<List<StationCordinateDto>>.SuccessResponse(stations));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<WeatherStationDto>>> GetById(Guid id)
    {
        var station = await _service.FindOneDtoAsync(x => x.Id == id);
        if (station == null)
            return NotFound(ApiResponse<WeatherStationDto>.FailureResponse("WeatherStation not found"));

        return Ok(ApiResponse<WeatherStationDto>.SuccessResponse(station));
    }

    [HttpPost(Name = "CreateStation")]
    public async Task<ActionResult<ApiResponse<WeatherStationDto>>> Create([FromBody] CreateWeatherStationRequest request)
    {
        if (!ModelState.IsValid)
            throw new Exception(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToString());

        var stationId = await _service.CreateAsync(request);
        return CreatedAtAction(
            nameof(GetById),
            new { id = stationId }
        );
    }

    [HttpPut("{id}", Name = "UpdateStation")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWeatherStationRequest request)
    {
        await _service.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteStation")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}