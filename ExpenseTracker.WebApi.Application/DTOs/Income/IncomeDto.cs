namespace ExpenseTracker.WebApi.Application.DTOs.Income;

public record IncomeDto(
    int Id,
    decimal Amount,
    string Description,
    DateTime TransactionDate,
    bool IsScheduled,
    int IncomeGroupId,
    string IncomeGroupName
);