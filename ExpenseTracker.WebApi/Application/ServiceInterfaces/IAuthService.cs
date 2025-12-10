using ExpenseTracker.WebApi.Application.DTOs.Auth;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<AuthResponse> LoginAsync(LoginRequest request);
}