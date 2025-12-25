namespace ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;

public record BudgetStatusDto(
    decimal? MonthlyLimit,
    decimal Spent,
    decimal Remaining,
    bool IsExceeded
);