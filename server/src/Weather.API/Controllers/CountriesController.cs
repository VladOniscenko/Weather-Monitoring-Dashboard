using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Weather.Application.Common.DTOs;
using Weather.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Weather.Infrastructure.Mappers;
using Weather.Application.Services;

namespace Weather.API.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class CountriesController : BaseController
{
    private readonly ICountryService _service;

    public CountriesController(ICountryService service)
    {
        _service = service;
    }

    [HttpGet(Name = "GetAllCountries")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<CountryDto>>>> GetAll([FromQuery] CountryQuery? query = null)
    {
        var countries = await _service.QueryAsync(query);
        return Ok(ApiResponse<List<CountryDto>>.SuccessResponse(countries));
    }

    [HttpGet("{id}", Name = "GetCountryById")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<CountryDto>>> GetById(Guid id)
    {
        var country = await _service.FindOneDtoAsync(x => x.Id == id);
        if (country == null)
            return NotFound(ApiResponse<CountryDto>.FailureResponse("Country not found"));

        return Ok(ApiResponse<CountryDto>.SuccessResponse(country));
    }

    [HttpPost(Name = "CreateCountry")]
    public async Task<ActionResult<ApiResponse<CountryDto>>> Create([FromBody] CreateCountryRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ApiResponse<CountryDto>.FailureResponse("Invalid data"));

        try
        {
            var result = await _service.CreateCountryAsync(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                ApiResponse<CountryDto>.SuccessResponse(result, "Country created successfully")
            );
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(ApiResponse<CountryDto>.FailureResponse("A country with this unique code (CCA2/CCA3) already exists."));
        }
    }

    [HttpPut("{id}", Name = "UpdateCountry")]
    public async Task<ActionResult<ApiResponse<CountryDto>>> Update(Guid id, [FromBody] UpdateCountryRequest request)
    {
        try
        {
            var result = await _service.UpdateCountryAsync(id, request);
            return Ok(ApiResponse<CountryDto>.SuccessResponse(result, "Country updated successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<CountryDto>.FailureResponse("Country not found"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<CountryDto>.FailureResponse(ex.Message));
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(ApiResponse<CountryDto>.FailureResponse("Update failed: CCA2 or CCA3 code is already in use by another country."));
        }
    }

    [HttpDelete("{id}", Name = "DeleteCountry")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
    {
        try
        {
            await _service.DeleteCountryAsync(id);
            return Ok(ApiResponse<object>.SuccessResponse(null, "Country deleted successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<object>.FailureResponse("Country not found"));
        }
    }
}