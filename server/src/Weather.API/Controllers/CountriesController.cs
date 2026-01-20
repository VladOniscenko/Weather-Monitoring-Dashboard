using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Weather.Domain.Entities;
using Weather.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Weather.Api.Controllers;

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
        return Ok(countries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var country = await _service.FindOneAsync(x => x.Id == id);

        if (country == null)
        {
            return NotFound();
        }

        return Ok(country);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Country country)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _service.CreateAsync(country);
            return CreatedAtAction(nameof(GetById), new { id = country.Id }, country);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(new { message = "A country with the same unique value already exists." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Country country)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != country.Id)
            return BadRequest();

        try
        {
            await _service.UpdateAsync(country);
            return NoContent();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Conflict(new { message = "A country with the same unique value already exists." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var country = await _service.FindOneAsync(x => x.Id == id);

        if (country == null)
        {
            return NotFound();
        }

        await _service.DeleteAsync(country);
        return NoContent();
    }
}
