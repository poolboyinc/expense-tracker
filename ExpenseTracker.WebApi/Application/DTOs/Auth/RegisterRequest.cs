using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.WebApi.Application.DTOs.Auth;

public class RegisterRequest
{
    [Required(ErrorMessage = "The name is required")]
    [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Incorrect email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must have at least 6 characters")]
    public string Password { get; set; } = string.Empty;
}