using ExpenseTracker.WebApi.Application.DTOs.Auth;

namespace ExpenseTracker.WebApi.Application.ServiceContracts;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    
    Task<AuthResponse> LoginAsync(LoginRequest request);
}