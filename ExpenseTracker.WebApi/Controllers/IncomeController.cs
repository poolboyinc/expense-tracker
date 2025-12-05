using System.Net;
using ExpenseTracker.WebApi.Application.ServiceContracts;
using ExpenseTracker.WebApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] 
public class IncomeController : ControllerBase
{
    private readonly IIncomeService _incomeService;
    private readonly IUserService _userService;

    public IncomeController(IIncomeService incomeService, IUserService userService)
    {
        _incomeService = incomeService;
        _userService = userService;
    }
    
    private string GetCurrentUserId()
    {
        return _userService.GetCurrentUserId(); 
    }


    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Income>))]
    public async Task<IActionResult> GetAllIncomes()
    {
        var userId = GetCurrentUserId();
        var incomes = await _incomeService.GetAllIncomesForUserAsync(userId);
        return Ok(incomes);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Income))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetIncome(int id)
    {
        var userId = GetCurrentUserId();
        var income = await _incomeService.GetIncomeByIdAsync(id, userId);

        if (income == null)
        {
            return NotFound();
        }

        return Ok(income);
    }


    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(Income))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateIncome([FromBody] Income income)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = GetCurrentUserId();

        try
        {
            var createdIncome = await _incomeService.CreateIncomeAsync(income, userId);
            return CreatedAtAction(nameof(GetIncome), new { id = createdIncome.Id }, createdIncome);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateIncome(int id, [FromBody] Income income)
    {
        if (id != income.Id || !ModelState.IsValid)
        {
            return BadRequest();
        }

        var userId = GetCurrentUserId();
        
        try
        {
            await _incomeService.UpdateIncomeAsync(income, userId);
            return NoContent(); 
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Income with ID {id} not found or unauthorized.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

 
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteIncome(int id)
    {
        var userId = GetCurrentUserId();
        
        var deleted = await _incomeService.DeleteIncomeAsync(id, userId);
        
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent(); 
    }
}