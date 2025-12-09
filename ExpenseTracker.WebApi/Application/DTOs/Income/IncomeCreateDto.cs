namespace ExpenseTracker.WebApi.Application.DTOs.Income;

public record IncomeCreateDto(
    decimal Amount,
    string Description,
    DateTime TransactionDate,
    bool IsScheduled,
    int IncomeGroupId
);
