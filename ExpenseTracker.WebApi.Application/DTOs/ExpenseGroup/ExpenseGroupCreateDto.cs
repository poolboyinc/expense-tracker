namespace ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;

public record ExpenseGroupCreateDto(
    string Name,
    decimal? MonthlyLimit
);