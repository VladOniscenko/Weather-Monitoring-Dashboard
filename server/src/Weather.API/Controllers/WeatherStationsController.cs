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
    public async Task<ActionResult<ApiResponse<WeatherStationDto>>> GetById(Guid id)
    {
        var station = await _service.FindOneDtoAsync(x => x.Id == id);

        if (station == null)
        {
            return NotFound(ApiResponse<WeatherStationDto>.FailureResponse("WeatherStation not found"));
        }

        return Ok(ApiResponse<WeatherStationDto>.SuccessResponse(station));
    }

    [HttpPost(Name = "CreateStation")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateWeatherStationRequest request)
    {
        if (!ModelState.IsValid) return BadRequestResponse("Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());

        try
        {
            var newWeatherStation = new WeatherStation(
                request.Name,
                request.Latitude,
                request.Longitude,
                request.CityId
            );

            await _service.CreateAsync(newWeatherStation);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newWeatherStation.Id },
                ApiResponse<WeatherStation>.SuccessResponse(newWeatherStation, "WeatherStation created successfully")
            );
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(ApiResponse<object>.FailureResponse("A station with this unique code (CCA2/CCA3) already exists."));
        }
    }

    [HttpPut("{id}", Name = "UpdateStation")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWeatherStationRequest request)
    {
        var station = await _service.FindOneAsync(x => x.Id == id);
        if (station == null)
            return NotFoundResponse("WeatherStation not found");

        try
        {
            station.UpdateDetails(
                request.Name,
                request.Latitude,
                request.Longitude,
                request.CityId
            );

            await _service.UpdateAsync(station);
            return OkResponse(station, "WeatherStation updated successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequestResponse(ex.Message);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(ApiResponse<object>.FailureResponse("Update failed: CCA2 or CCA3 code is already in use by another station."));
        }
    }

    [HttpDelete("{id}", Name = "DeleteStation")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var station = await _service.FindOneAsync(x => x.Id == id);
        if (station == null)
            return NotFoundResponse("WeatherStation not found");

        await _service.DeleteAsync(station);
        return OkResponse<object>("", "WeatherStation deleted successfully");
    }
}