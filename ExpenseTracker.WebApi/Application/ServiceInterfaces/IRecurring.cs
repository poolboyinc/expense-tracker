using ExpenseTracker.WebApi.Domain.Enums;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IRecurring
{
    DateTime NextRunAt { get; set; }
    RecurrenceFrequency Frequency { get; }
    int? DayOfMonth { get; }
}
