using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Weather.Domain.Entities;
using Weather.Application.Services;

namespace Weather.Api.Controllers;

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
    public async Task<IActionResult> GetAll([FromQuery] CityQuery? query = null)
    {
        var countries = await _service.QueryAsync(query);
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
    public async Task<IActionResult> Create([FromBody] City city)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _service.CreateAsync(city);
            return CreatedAtAction(nameof(GetById), new { id = city.Id }, city);
        }
        catch
        {
            return StatusCode(500, "An error occurred while creating the city.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] City city)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != city.Id)
            return BadRequest();

        try
        {
            await _service.UpdateAsync(city);
            return NoContent();
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
