using ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpenseGroupsController(IExpenseGroupService groupService, IUserServiceContext userServiceContext)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ExpenseGroupListDto>>> GetAllGroups()
    {
        var groups = await groupService.GetAllGroupsForUserAsync();

        return Ok(groups);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpenseGroupDetailsDto>> GetGroup(int id)
    {
        var group = await groupService.GetGroupByIdAsync(id);

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }
    
    [HttpGet("{id}/budget-status")]
    public async Task<ActionResult<BudgetStatusDto>> GetBudgetStatus(int id)
    {
        return Ok(await groupService.GetBudgetStatusAsync(id));
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ExpenseGroupDetailsDto>> CreateGroup([FromBody] ExpenseGroupCreateDto dto)
    {
        try
        {
            var created = await groupService.CreateGroupAsync(dto);
            return CreatedAtAction(nameof(GetGroup), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGroup(int id, [FromBody] ExpenseGroupUpdateDto dto)
    {
        var userId = userServiceContext.GetCurrentUserId();

        var existing = await groupService.GetGroupByIdAsync(id);

        if (existing == null)
        {
            return NotFound("Expense group not found or unauthorized.");
        }


        await groupService.UpdateGroupAsync(id, dto);
        return NoContent();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        try
        {
            var deleted = await groupService.DeleteGroupAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { ex.Message });
        }
    }
}