namespace ExpenseTracker.WebApi.Application.DTOs.Savings;

public record SavingsPlanCreateDto(
    decimal TargetAmount,
    DateTime TargetDate
);
