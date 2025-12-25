using ExpenseTracker.WebApi.Domain.Enums;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IRecurring
{
    DateTime NextRunAt { get; set; }
    RecurrenceFrequency Frequency { get; }
    int? DayOfMonth { get; }
}
