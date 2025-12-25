using ExpenseTracker.WebApi.Application.DTOs.IncomeGroup;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IncomeGroupController(IIncomeGroupService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await service.GetAllForUserAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var group = await service.GetByIdAsync(id);
        return group == null ? NotFound() : Ok(group);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IncomeGroupCreateDto dto)
    {
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] IncomeGroupUpdateDto dto)
    {
        try
        {
            await service.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}