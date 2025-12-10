using ExpenseTracker.WebApi.Application.DTOs.Expense;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExpenseController(IExpenseService expenseService, IUserServiceContext userServiceContext)
    : ControllerBase
{
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ExpenseListDto>>> GetExpenses([FromQuery] ExpenseQueryParameters parameters)
    {
        var userId = userServiceContext.GetCurrentUserId();

        var expenses = await expenseService.GetFilteredExpensesAsync(
            userId,
            parameters.GroupId,
            parameters.SearchTerm,
            parameters.PageNumber,
            parameters.PageSize
        );

        return Ok(expenses);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpenseDetailsDto>> GetExpenseById(int id)
    {
        var expense = await expenseService.GetExpenseByIdAsync(id);

        if (expense == null)
        {
            return NotFound("Expense was not found");
        }

        return Ok(expense);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ExpenseDetailsDto>> CreateExpense([FromBody] ExpenseCreateDto dto)
    {
        
        try
        {
            var created = await expenseService.CreateExpenseAsync(dto);

            return CreatedAtAction(
                nameof(GetExpenseById),
                new { id = created.Id },
                created
            );
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseUpdateDto dto)
    {
        try
        {
            await expenseService.UpdateExpenseAsync(dto);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var userId = userServiceContext.GetCurrentUserId();

        try
        {
            await expenseService.DeleteExpenseAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Expense not found");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid(); 
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}


