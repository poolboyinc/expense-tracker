using System.Security.Claims;
using ExpenseTracker.WebApi.Application.DTOs.User;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> GetMe()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await userService.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(user);
    }


    [HttpDelete("me")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMe()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var deleted = await userService.DeleteUserAsync(userId);
        if (!deleted)
        {
            return NotFound("User not found.");
        }

        return NoContent();
    }
}