namespace ExpenseTracker.WebApi.Application.DTOs.User;

public record UpgradeToPremiumDto(
    string Token,
    bool IsPremium);