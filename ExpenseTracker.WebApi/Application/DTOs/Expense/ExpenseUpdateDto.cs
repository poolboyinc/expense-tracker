namespace ExpenseTracker.WebApi.Application.DTOs.Expense;

public record ExpenseUpdateDto(
    decimal Amount,
    string Description,
    DateTime Date,
    int ExpenseGroupId
);