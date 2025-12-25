using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Enums;

namespace ExpenseTracker.WebApi.Application.DTOs.ScheduledExpense;

public record ScheduledExpenseUpdateDto(
    int Id,
    decimal Amount,
    string Description,
    DateTime NextRunAt,
    RecurrenceFrequency Frequency,
    int? DayOfWeek,
    int? DayOfMonth,
    DateTime? EndAt,
    bool IsActive,
    int ExpenseGroupId
);
