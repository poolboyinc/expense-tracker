namespace ExpenseTracker.WebApi.Application.DTOs.Expense;

public record ExpenseUpdateDto(
    int Id,
    decimal Amount,
    string Description,
    DateTime Date,
    int ExpenseGroupId
);