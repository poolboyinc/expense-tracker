using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.WebApi.Application.DTOs.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "Email field is required")]
    [EmailAddress(ErrorMessage = "Incorrect email format")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}