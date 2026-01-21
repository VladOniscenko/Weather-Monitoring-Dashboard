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
public class CountriesController : BaseController
{
    private readonly IGenericService<Country> _service;

    public CountriesController(IGenericService<Country> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var countries = await _service.GetAllAsync();
        return OkResponse(countries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var country = await _service.FindOneAsync(x => x.Id == id);

        if (country == null)
        {
            return NotFoundResponse("Country not found");
        }

        return OkResponse(country);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCountryRequest request)
    {
        if (!ModelState.IsValid) return BadRequestResponse("Invalid data", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());

        try
        {
            var newCountry = new Country(
                request.Name,
                request.CCA2,
                request.CCA3,
                request.Region,
                request.Subregion,
                request.Capital,
                request.Flag,
                request.Latitude,
                request.Longitude,
                request.Independent,
                request.Landlocked
            );

            await _service.CreateAsync(newCountry);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newCountry.Id },
                ApiResponse<Country>.SuccessResponse(newCountry, "Country created successfully")
            );
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(ApiResponse<object>.FailureResponse("A country with this unique code (CCA2/CCA3) already exists."));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCountryRequest request)
    {
        var country = await _service.FindOneAsync(x => x.Id == id);
        if (country == null)
            return NotFoundResponse("Country not found");

        try
        {
            country.UpdateDetails(
                request.Name,
                request.CCA2,
                request.CCA3,
                request.Region,
                request.Subregion,
                request.Capital,
                request.Flag,
                request.Latitude,
                request.Longitude,
                request.Independent,
                request.Landlocked
            );

            await _service.UpdateAsync(country);
            return OkResponse(country, "Country updated successfully");
        }
        catch (ArgumentException ex)
        {
            return BadRequestResponse(ex.Message);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(ApiResponse<object>.FailureResponse("Update failed: CCA2 or CCA3 code is already in use by another country."));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var country = await _service.FindOneAsync(x => x.Id == id);
        if (country == null)
            return NotFoundResponse("Country not found");

        await _service.DeleteAsync(country);
        return OkResponse<object>("", "Country deleted successfully");
    }
}