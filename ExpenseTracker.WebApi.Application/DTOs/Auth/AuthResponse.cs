using ExpenseTracker.WebApi.Application.DTOs.User;

namespace ExpenseTracker.WebApi.Application.DTOs.Auth;

public record AuthResponse(UserDto User, string Token, DateTime ExpiresAt);