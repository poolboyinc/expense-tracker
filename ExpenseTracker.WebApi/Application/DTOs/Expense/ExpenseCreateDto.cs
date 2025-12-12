namespace ExpenseTracker.WebApi.Application.DTOs.Expense;

public record ExpenseCreateDto(
    decimal Amount,
    string Description,
    DateTime Date,
    int ExpenseGroupId
);