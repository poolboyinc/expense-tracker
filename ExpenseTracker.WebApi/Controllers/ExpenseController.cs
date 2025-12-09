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
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    private readonly IUserServiceContext _userServiceContext;
    
    public class ExpenseParameters
    {
        public int? GroupId { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        //public string SortBy { get; set; } = "date"; 
    }

    public ExpenseController(IExpenseService expenseService, IUserServiceContext userServiceContext)
    {
        _expenseService = expenseService;
        _userServiceContext = userServiceContext;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ExpenseListDto>>> GetExpenses([FromQuery] ExpenseParameters parameters)
    {
        var userId = _userServiceContext.GetCurrentUserId();

        var expenses = await _expenseService.GetFilteredExpensesAsync(
            userId,
            parameters.GroupId,
            parameters.SearchTerm,
            parameters.PageNumber,
            parameters.PageSize
        );

        return Ok(expenses.Select(e => e.ToListDto()));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpenseDetailsDto>> GetExpenseById(int id)
    {
        var userId = _userServiceContext.GetCurrentUserId();

        var expense = await _expenseService.GetExpenseByIdAsync(id, userId);

        if (expense == null)
            return NotFound("Expense was not found");

        return Ok(expense.ToDetailsDto());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ExpenseDetailsDto>> CreateExpense([FromBody] ExpenseCreateDto dto)
    {
        var userId = _userServiceContext.GetCurrentUserId();

        var newExpense = dto.ToEntity(userId);

        try
        {
            var created = await _expenseService.CreateExpenseAsync(newExpense, userId);

            return CreatedAtAction(
                nameof(GetExpenseById),
                new { id = created.Id },
                created.ToDetailsDto()
            );
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseUpdateDto dto)
    {
        var userId = _userServiceContext.GetCurrentUserId();

        var exp = dto.ToEntity(id, userId);

        try
        {
            await _expenseService.UpdateExpenseAsync(exp, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound("Expense was not found");
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var userId = _userServiceContext.GetCurrentUserId();

        try
        {
            await _expenseService.DeleteExpenseAsync(id, userId);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound("Expense was not found");
        }
    }
}


