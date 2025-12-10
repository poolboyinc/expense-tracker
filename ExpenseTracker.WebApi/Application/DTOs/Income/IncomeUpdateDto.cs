namespace ExpenseTracker.WebApi.Application.DTOs.Income;

public record IncomeUpdateDto(
    decimal Amount,
    string Description,
    DateTime TransactionDate,
    bool IsScheduled,
    int IncomeGroupId
);
