namespace ExpenseTracker.WebApi.Application.DTOs.Savings;

public record SavingsPlanDto(
    int Id,
    decimal TargetAmount,
    DateTime TargetDate,
    bool IsCompleted,
    IReadOnlyCollection<SavingsPlanContributionDto> Contributions
);
