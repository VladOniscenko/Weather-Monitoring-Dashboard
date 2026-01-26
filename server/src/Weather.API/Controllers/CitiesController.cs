using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Weather.Domain.Entities;
using Weather.Application.Services;
using Weather.Application.Common.DTOs;
using Weather.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Weather.Infrastructure.Mappers;
using Weather.Application.Common.Interfaces;

namespace Weather.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class CitiesController : BaseController
{
    private readonly ICityService _service;

    public CitiesController(ICityService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<CityDto>>>> GetAll([FromQuery] CityQuery? query = null)
    {
        var cities = await _service.QueryAsync(query);
        return Ok(ApiResponse<List<CityDto>>.SuccessResponse(cities));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<CityDto>>> GetById(Guid id)
    {
        var city = await _service.FindOneDtoAsync(x => x.Id == id);
        if (city == null)
            return NotFound(ApiResponse<CityDto>.FailureResponse("City not found"));

        return Ok(ApiResponse<CityDto>.SuccessResponse(city));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CityDto>>> Create([FromBody] CreateCityRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<CityDto>.FailureResponse("Invalid data"));

        try
        {
            var result = await _service.CreateCityAsync(request);
            return CreatedAtAction(
                nameof(GetById), 
                new { id = result.Id }, 
                ApiResponse<CityDto>.SuccessResponse(result, "City created successfully")
            );
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(ApiResponse<CityDto>.FailureResponse("A city with the same name already exists in this country."));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<CityDto>.FailureResponse(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<CityDto>>> Update(Guid id, [FromBody] UpdateCityRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<CityDto>.FailureResponse("Invalid data"));

        try
        {
            var result = await _service.UpdateCityAsync(id, request);
            return Ok(ApiResponse<CityDto>.SuccessResponse(result, "City updated successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<CityDto>.FailureResponse("City not found"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<CityDto>.FailureResponse(ex.Message));
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(ApiResponse<CityDto>.FailureResponse("Update failed: City name already exists."));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
    {
        try
        {
            await _service.DeleteCityAsync(id);
            return Ok(ApiResponse<object>.SuccessResponse(null, "City deleted successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<object>.FailureResponse("City not found"));
        }
    }
}