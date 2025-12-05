using System.Net;
using ExpenseTracker.WebApi.Application.ServiceContracts;
using ExpenseTracker.WebApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize] 
public class ExpenseGroupsController : ControllerBase
{
    private readonly IExpenseGroupService _groupService;
    private readonly IUserService _userService; 

    public ExpenseGroupsController(IExpenseGroupService groupService, IUserService userService)
    {
        _groupService = groupService;
        _userService = userService;
    }
    
    private string GetCurrentUserId()
    {
        //in the future we will use jwt tokens
        return _userService.GetCurrentUserId(); 
    }


    [HttpGet]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<ExpenseGroup>))]
    public async Task<IActionResult> GetAllGroups()
    {
        var userId = GetCurrentUserId();
        var groups = await _groupService.GetAllGroupsForUserAsync(userId);
        return Ok(groups);
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ExpenseGroup))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetGroup(int id)
    {
        var userId = GetCurrentUserId();
        var group = await _groupService.GetGroupByIdAsync(id, userId);

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(ExpenseGroup))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateGroup([FromBody] ExpenseGroup group)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var userId = GetCurrentUserId();

        try
        {
            var createdGroup = await _groupService.CreateGroupAsync(group, userId);
            return CreatedAtAction(nameof(GetGroup), new { id = createdGroup.Id }, createdGroup);
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
    public async Task<IActionResult> UpdateGroup(int id, [FromBody] ExpenseGroup group)
    {
        if (id != group.Id || !ModelState.IsValid)
        {
            return BadRequest();
        }

        var userId = GetCurrentUserId();

        try
        {
            await _groupService.UpdateGroupAsync(group, userId);
            return NoContent(); 
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Expense Group with ID {id} not found or unauthorized.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }


    [HttpDelete("{id}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.Conflict)] 
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var userId = GetCurrentUserId();

        try
        {
            var deleted = await _groupService.DeleteGroupAsync(id, userId);
            
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message }); 
        }
    }
}