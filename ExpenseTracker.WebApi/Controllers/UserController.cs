using ExpenseTracker.WebApi.Application.ServiceContracts;
using ExpenseTracker.WebApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    
    public class UserInputModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("current")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<string>> GetCurrentUserId()
    {
        var userId = await _userService.GetCurrentUserIdAsync();
        return Ok(userId);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)] 
    public async Task<ActionResult<User>> CreateUser([FromBody] UserInputModel input)
    {
        if (string.IsNullOrEmpty(input.Id))
        {
            return BadRequest("ID korisnika je obavezan.");
        }
        
        var user = new User
        {
            Id = input.Id,
            Name = input.Name
        };

        try
        {
            var createdUser = await _userService.CreateUserAsync(user);
            
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUserById(string id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound($"Korisnik sa ID-jem {id} nije pronađen.");
        }

        return Ok(user);
    }
}