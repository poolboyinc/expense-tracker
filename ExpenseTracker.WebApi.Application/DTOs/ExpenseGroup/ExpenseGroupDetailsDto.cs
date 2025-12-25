namespace ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;

public record ExpenseGroupDetailsDto(
    int Id,
    string Name,
    decimal? MonthlyLimit,
    int ExpensesCount
);