using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Enums;

namespace ExpenseTracker.WebApi.Application.DTOs.ScheduledExpense;

public record ScheduledExpenseCreateDto(
    decimal Amount,
    string Description,
    DateTime FirstRunAt,
    RecurrenceFrequency Frequency,
    int? DayOfWeek,
    int? DayOfMonth,
    DateTime? EndAt,
    int ExpenseGroupId
);
