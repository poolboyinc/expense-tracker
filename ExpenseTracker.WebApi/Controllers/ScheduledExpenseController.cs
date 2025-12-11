using ExpenseTracker.WebApi.Application.DTOs.ScheduledExpense;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScheduledExpensesController(IScheduledExpenseService svc) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduledExpenseDto>>> GetAll()
    {
        var list = await svc.GetAllForCurrentUserAsync();
        return Ok(list);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduledExpenseDto>> Get(int id)
    {
        var dto = await svc.GetByIdAsync(id);
        if (dto == null)
        {
            return NotFound();
        }

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ScheduledExpenseDto>> Create([FromBody] ScheduledExpenseCreateDto dto)
    {
        var created = await svc.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ScheduledExpenseUpdateDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest("Id mismatch");
        }

        await svc.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await svc.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("process-now")]
    public async Task<IActionResult> ProcessNow()
    {
        var count = await svc.ProcessDueScheduledExpensesAsync(DateTime.UtcNow);
        return Ok(new { created = count });
    }
}