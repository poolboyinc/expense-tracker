namespace ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;

public record ExpenseGroupUpdateDto(
    string Name,
    decimal? MonthlyLimit
);