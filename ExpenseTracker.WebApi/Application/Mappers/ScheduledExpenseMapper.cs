using ExpenseTracker.WebApi.Application.DTOs.ScheduledExpense;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.Mappers;

public static class ScheduledExpenseMapper
{
    public static ScheduledExpense ToEntity(this ScheduledExpenseCreateDto dto, Guid userId) =>
        new ScheduledExpense
        {
            Amount = dto.Amount,
            Description = dto.Description,
            NextRunAt = dto.FirstRunAt,
            Frequency = dto.Frequency,
            DayOfWeek = dto.DayOfWeek,
            DayOfMonth = dto.DayOfMonth,
            EndAt = dto.EndAt,
            ExpenseGroupId = dto.ExpenseGroupId,
            UserId = userId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

    public static void MapUpdateToEntity(this ScheduledExpenseUpdateDto dto, ScheduledExpense entity)
    {
        entity.Amount = dto.Amount;
        entity.Description = dto.Description;
        entity.NextRunAt = dto.NextRunAt;
        entity.Frequency = dto.Frequency;
        entity.DayOfWeek = dto.DayOfWeek;
        entity.DayOfMonth = dto.DayOfMonth;
        entity.EndAt = dto.EndAt;
        entity.IsActive = dto.IsActive;
        entity.ExpenseGroupId = dto.ExpenseGroupId;
    }

    public static ScheduledExpenseDto ToDto(this ScheduledExpense e) =>
        new ScheduledExpenseDto(
            e.Id,
            e.Amount,
            e.Description,
            e.NextRunAt,
            e.Frequency,
            e.DayOfWeek,
            e.DayOfMonth,
            e.EndAt,
            e.IsActive,
            e.ExpenseGroupId
        );
}
