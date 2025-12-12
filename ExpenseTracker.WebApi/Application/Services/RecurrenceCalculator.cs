using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Enums;

namespace ExpenseTracker.WebApi.Application.Services;

public static class RecurrenceCalculator
{
    public static DateTime? ComputeNextRun(ScheduledExpense s)
    {
        var current = s.NextRunAt;
        return s.Frequency switch
        {
            RecurrenceFrequency.Once => null,
            RecurrenceFrequency.Daily => current.AddDays(1),
            RecurrenceFrequency.Weekly => current.AddDays(7),
            RecurrenceFrequency.Monthly =>
                s.DayOfMonth.HasValue
                    ? NextMonthDay(current, s.DayOfMonth.Value)
                    : current.AddMonths(1),
            _ => null
        };
    }

    private static DateTime NextMonthDay(DateTime baseDate, int dayOfMonth)
    {
        var next = baseDate.AddMonths(1);
        var daysInMonth = DateTime.DaysInMonth(next.Year, next.Month);
        var safeDay = Math.Min(dayOfMonth, daysInMonth);

        return new DateTime(next.Year, next.Month, safeDay,
            baseDate.Hour, baseDate.Minute, baseDate.Second, DateTimeKind.Utc);
    }
}
