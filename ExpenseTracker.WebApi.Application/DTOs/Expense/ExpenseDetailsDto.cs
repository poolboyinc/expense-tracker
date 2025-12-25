namespace ExpenseTracker.WebApi.Application.DTOs.Expense;

public record ExpenseDetailsDto(
    int Id,
    decimal Amount,
    string Description,
    DateTime TransactionDate,
    int ExpenseGroupId,
    string ExpenseGroupName,
    bool IsScheduled
);