using System.Net;
using ExpenseTracker.WebApi.Application.DTOs.Income;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class IncomeController(IIncomeService incomeService, IUserServiceContext userServiceContext)
    : ControllerBase
{
    
    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Income>))]
    public async Task<IActionResult> GetAllIncomes()
    {
        var incomes = await incomeService.GetAllIncomesForUserAsync();
        return Ok(incomes);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Income))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetIncome(int id)
    {
        var userId = userServiceContext.GetCurrentUserId();
        var income = await incomeService.GetIncomeByIdAsync(id);
        
        if (income == null)
        {
            return NotFound();
        }
        
        return Ok(income);
    }


    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(Income))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateIncome([FromBody] IncomeCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var createdIncome = await incomeService.CreateIncomeAsync(dto);
        return CreatedAtAction(nameof(GetIncome), new { id = createdIncome.Id }, createdIncome);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateIncome(int id, [FromBody] IncomeUpdateDto dto)
    {
        if (id != dto.Id || !ModelState.IsValid)
        {
            return BadRequest();
        }
        
        await incomeService.UpdateIncomeAsync(dto);
        
        return NoContent();
    }

 
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteIncome(int id)
    {
        var userId = userServiceContext.GetCurrentUserId();
        var deleted = await incomeService.DeleteIncomeAsync(id);

        if (!deleted)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}