using ExpenseTracker.WebApi.Application.DTOs.Income;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.Mappers;

public static class IncomeMapper
{
    public static IncomeDto ToDto(Income income)
    {
        return new IncomeDto(
            income.Id,
            income.Amount,
            income.Description,
            income.Date,
            income.IsScheduled,
            income.IncomeGroupId,
            income.IncomeGroup?.Name ?? string.Empty
        );
    }

    public static Income FromCreateDto(IncomeCreateDto dto)
    {
        return new Income
        {
            Amount = dto.Amount,
            Description = dto.Description,
            Date = dto.TransactionDate,
            IsScheduled = dto.IsScheduled,
            IncomeGroupId = dto.IncomeGroupId
        };
    }

    public static void UpdateEntity(Income income, IncomeUpdateDto dto)
    {
        income.Amount = dto.Amount;
        income.Description = dto.Description;
        income.Date = dto.TransactionDate;
        income.IsScheduled = dto.IsScheduled;
        income.IncomeGroupId = dto.IncomeGroupId;
    }
}