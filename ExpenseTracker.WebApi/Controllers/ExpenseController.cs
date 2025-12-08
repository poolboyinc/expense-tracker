using ExpenseTracker.WebApi.Application.ServiceContracts;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

 
[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    private readonly IUserService _userService;
    
    public class ExpenseParameters
    {
        public int? GroupId { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        //public string SortBy { get; set; } = "date"; 
    }
    
    public class ExpenseInputModel
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int ExpenseGroupId { get; set; } 
    }

    public ExpenseController(IExpenseService expenseService, IUserService userService)
    {
        _expenseService = expenseService;
        _userService = userService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses(
        [FromQuery] ExpenseParameters parameters)
    {
        var userId = await _userService.GetCurrentUserIdAsync();
        
        var expenses = await _expenseService.GetFilteredExpensesAsync(
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
    public async Task<ActionResult<Expense>> GetExpenseById(int id)
    {
        var userId = await _userService.GetCurrentUserIdAsync();
        
        var expense = await _expenseService.GetExpenseByIdAsync(id, userId);

        if (expense == null)
        {
            return NotFound("Expense was not found");
        }

        return Ok(expense);
    }
    

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Expense>> CreateExpense(
        [FromBody] ExpenseInputModel input)
    {
        var userId = await _userService.GetCurrentUserIdAsync();
        
        var newExpense = new Expense
        {
            Amount = input.Amount,
            Description = input.Description,
            TransactionDate = input.Date,
            ExpenseGroupId = input.ExpenseGroupId,
        };

        try
        {
            var createdExpense = await _expenseService.CreateExpenseAsync(newExpense, userId);
            
            return CreatedAtAction(nameof(GetExpenseById), new { id = createdExpense.Id }, createdExpense);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseInputModel input)
    {
        if (id <= 0) return BadRequest("Expense ID is invalid.");

        var userId = await _userService.GetCurrentUserIdAsync();
        
        var expenseToUpdate = new Expense
        {
            Id = id,
            Amount = input.Amount,
            Description = input.Description,
            TransactionDate = input.Date,
            ExpenseGroupId = input.ExpenseGroupId,
        };

        try
        {
            await _expenseService.UpdateExpenseAsync(expenseToUpdate, userId);
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var userId = await _userService.GetCurrentUserIdAsync();

        try
        {
            await _expenseService.DeleteExpenseAsync(id, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return NotFound("Expense was not found");
        }
    }
}


