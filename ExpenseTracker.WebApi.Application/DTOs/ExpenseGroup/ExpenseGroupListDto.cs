namespace ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;

public record ExpenseGroupListDto(
    int Id,
    string Name,
    decimal? MonthlyLimit,
    int ExpensesCount
);