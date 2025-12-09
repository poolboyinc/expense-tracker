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
public class IncomeController : ControllerBase
{
    private readonly IIncomeService _incomeService;
    private readonly IUserServiceContext _userServiceContext;

    public IncomeController(IIncomeService incomeService, IUserServiceContext userServiceContext)
    {
        _incomeService = incomeService;
        _userServiceContext = userServiceContext;
    }
    
    private string GetCurrentUserId()
    {
        return _userServiceContext.GetCurrentUserId(); 
    }


    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Income>))]
    public async Task<IActionResult> GetAllIncomes()
    {
        var userId = GetCurrentUserId();
        var incomes = await _incomeService.GetAllIncomesForUserAsync(userId);
        var dtoList = incomes.Select(IncomeMapper.ToDto);
        return Ok(dtoList);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Income))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetIncome(int id)
    {
        var userId = GetCurrentUserId();
        var income = await _incomeService.GetIncomeByIdAsync(id, userId);
        if (income == null) return NotFound();
        return Ok(IncomeMapper.ToDto(income));
    }


    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(Income))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateIncome([FromBody] IncomeCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = GetCurrentUserId();
        var income = IncomeMapper.FromCreateDto(dto);
        var createdIncome = await _incomeService.CreateIncomeAsync(income, userId);
        return CreatedAtAction(nameof(GetIncome), new { id = createdIncome.Id }, IncomeMapper.ToDto(createdIncome));
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> UpdateIncome(int id, [FromBody] IncomeUpdateDto dto)
    {
        if (id != dto.Id || !ModelState.IsValid) return BadRequest();
        var userId = GetCurrentUserId();
        var existingIncome = await _incomeService.GetIncomeByIdAsync(id, userId);
        if (existingIncome == null) return NotFound();

        IncomeMapper.UpdateEntity(existingIncome, dto);
        await _incomeService.UpdateIncomeAsync(existingIncome, userId);
        return NoContent();
    }

 
    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteIncome(int id)
    {
        var userId = GetCurrentUserId();
        var deleted = await _incomeService.DeleteIncomeAsync(id, userId);
        if (!deleted) return NotFound();
        return NoContent();
    }
}