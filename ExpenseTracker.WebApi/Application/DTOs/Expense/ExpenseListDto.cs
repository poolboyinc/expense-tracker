namespace ExpenseTracker.WebApi.Application.DTOs.Expense;

public record ExpenseListDto(
    int Id,
    decimal Amount,
    string Description,
    DateTime TransactionDate,
    int ExpenseGroupId,
    string ExpenseGroupName
);