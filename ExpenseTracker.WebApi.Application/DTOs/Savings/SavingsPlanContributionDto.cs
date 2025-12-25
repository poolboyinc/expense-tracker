namespace ExpenseTracker.WebApi.Application.DTOs.Savings;

public record SavingsPlanContributionDto(
    DateTime Month,
    decimal PlannedAmount,
    decimal ActualAmount
);
