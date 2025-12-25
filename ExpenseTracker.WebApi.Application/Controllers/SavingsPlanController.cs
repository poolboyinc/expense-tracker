using ExpenseTracker.WebApi.Application.DTOs.Savings;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[Authorize(Policy = "Premium")]
[ApiController]
[Route("api/[controller]")]
public class SavingsPlanController(ISavingsPlanService savingsPlanService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<SavingsPlanDto>> Create(
        [FromBody] SavingsPlanCreateDto dto)
    {
        var result = await savingsPlanService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<SavingsPlanDto>>> GetAll()
    {
        var plans = await savingsPlanService.GetAllAsync();
        return Ok(plans);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SavingsPlanDto>> GetById(int id)
    {
        var plan = await savingsPlanService.GetByIdAsync(id);
        if (plan == null)
        {
            return NotFound();
        }

        return Ok(plan);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await savingsPlanService.DeleteAsync(id);
        return NoContent();
    }
}