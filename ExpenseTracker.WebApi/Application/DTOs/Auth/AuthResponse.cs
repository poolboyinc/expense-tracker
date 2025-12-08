namespace ExpenseTracker.WebApi.Application.DTOs.Auth;

public class AuthResponse
{
    public string UserId { get; set; } = string.Empty;
    
    public string UserName { get; set; } = string.Empty;
    
    public string Token { get; set; } = string.Empty;
    
    public DateTime ExpiresAt { get; set; }
}