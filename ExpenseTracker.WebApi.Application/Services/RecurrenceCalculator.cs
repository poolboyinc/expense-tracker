using ExpenseTracker.WebApi.Domain.Enums;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public static class RecurrenceCalculator
{
    public static DateTime? ComputeNextRun(IRecurring r)
    {
        var current = r.NextRunAt;

        return r.Frequency switch
        {
            RecurrenceFrequency.Once => null,
            RecurrenceFrequency.Daily => current.AddDays(1),
            RecurrenceFrequency.Weekly => current.AddDays(7),
            RecurrenceFrequency.Monthly =>
                r.DayOfMonth.HasValue
                    ? NextMonthDay(current, r.DayOfMonth.Value)
                    : current.AddMonths(1),
            _ => null
        };
    }

    private static DateTime NextMonthDay(DateTime baseDate, int dayOfMonth)
    {
        var next = baseDate.AddMonths(1);
        var daysInMonth = DateTime.DaysInMonth(next.Year, next.Month);
        var day = Math.Min(dayOfMonth, daysInMonth);

        return new DateTime(
            next.Year,
            next.Month,
            day,
            baseDate.Hour,
            baseDate.Minute,
            baseDate.Second,
            DateTimeKind.Utc);
    }
}
