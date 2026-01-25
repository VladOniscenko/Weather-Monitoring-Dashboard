using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Weather.Domain.Entities;
using Weather.Application.Services;
using Weather.Application.Common.DTOs;
using Weather.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Weather.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class CitiesController : BaseController
{
    private readonly CityService _service;

    public CitiesController(CityService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] CityQuery? query = null)
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
            return NotFoundResponse("City not found");

        return OkResponse(city);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCityRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var newCity = new City(
                request.CountryId,
                request.Name,
                request.Latitude,
                request.Longitude,
                request.Timezone,
                request.Population
            );
            
            await _service.CreateAsync(newCity);
            return CreatedAtAction(
                nameof(GetById), 
                new { id = newCity.Id }, 
                ApiResponse<City>.SuccessResponse(newCity, "City created successfully")
            );
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(ApiResponse<object>.FailureResponse("A city with the same name already exists in this country."));
        }
        catch (ArgumentException ex)
        {
            return BadRequestResponse(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCityRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var city = await _service.FindOneAsync(x => x.Id == id);
        if (city == null) return NotFoundResponse("City not found");

        try
        {
            city.UpdateDetails(request.Name, request.Population, request.Timezone);
            await _service.UpdateAsync(city);
            return OkResponse(city, "City updated successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequestResponse(ex.Message);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(ApiResponse<object>.FailureResponse("Update failed: City name already exists."));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var city = await _service.FindOneAsync(x => x.Id == id);
        if (city == null)
            return NotFoundResponse("City not found");

        await _service.DeleteAsync(city);
        return OkResponse<object>(null, "City deleted successfully");
    }
}