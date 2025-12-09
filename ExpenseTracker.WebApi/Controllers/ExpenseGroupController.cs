using System.ComponentModel.DataAnnotations;
using System.Net;
using ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpenseGroupsController(IExpenseGroupService groupService, IUserServiceContext userServiceContext)
    : ControllerBase
{
    private string GetCurrentUserId() => userServiceContext.GetCurrentUserId();


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ExpenseGroupListDto>>> GetAllGroups()
    {
        var userId = GetCurrentUserId();
        var groups = await groupService.GetAllGroupsForUserAsync(userId);

        return Ok(groups.Select(g => g.ToListDto()));
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpenseGroupDetailsDto>> GetGroup(int id)
    {
        var userId = GetCurrentUserId();
        var group = await groupService.GetGroupByIdAsync(id, userId);

        if (group == null)
            return NotFound();

        return Ok(group.ToDetailsDto());
    }

   
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ExpenseGroupDetailsDto>> CreateGroup([FromBody] ExpenseGroupCreateDto dto)
    {
        var userId = GetCurrentUserId();

        var group = dto.ToEntity(userId);

        try
        {
            var created = await groupService.CreateGroupAsync(group, userId);
            return CreatedAtAction(nameof(GetGroup), new { id = created.Id }, created.ToDetailsDto());
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGroup(int id, [FromBody] ExpenseGroupUpdateDto dto)
    {
        var userId = GetCurrentUserId();
        var existing = await groupService.GetGroupByIdAsync(id, userId);

        if (existing == null)
            return NotFound("Expense group not found or unauthorized.");

        dto.MapToEntity(existing);

        await groupService.UpdateGroupAsync(existing, userId);
        return NoContent();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var userId = GetCurrentUserId();

        try
        {
            var deleted = await groupService.DeleteGroupAsync(id, userId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
    }
}
