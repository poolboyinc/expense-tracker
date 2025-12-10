using ExpenseTracker.WebApi.Application.DTOs.Expense;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.Mappers;

public static class ExpenseMapper
{
    public static Expense ToEntity(this ExpenseCreateDto dto, string userId)
    {
        return new Expense()
        {
            Amount = dto.Amount,
            Description = dto.Description,
            TransactionDate = dto.Date,
            ExpenseGroupId = dto.ExpenseGroupId,
            UserId = userId
        };
    }

    public static Expense ToEntity(this ExpenseUpdateDto dto, int id, string userId)
    {
        return new Expense()
        {
            Id = id,
            Amount = dto.Amount,
            Description = dto.Description,
            TransactionDate = dto.Date,
            ExpenseGroupId = dto.ExpenseGroupId,
            UserId = userId
        };
    }

    public static ExpenseDetailsDto ToDetailsDto(this Expense e)
    {
        return new ExpenseDetailsDto(
            e.Id,
            e.Amount,
            e.Description,
            e.TransactionDate,
            e.ExpenseGroupId,
            e.ExpenseGroup?.Name ?? "",
            e.IsScheduled
        );
    }

    public static ExpenseListDto ToListDto(this Expense e)
    {
        return new ExpenseListDto(
            e.Id,
            e.Amount,
            e.Description,
            e.TransactionDate,
            e.ExpenseGroupId,
            e.ExpenseGroup?.Name ?? ""
        );
    }
}