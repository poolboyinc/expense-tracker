namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IScheduledIncomeService
{
    Task<int> ProcessDueScheduledIncomesAsync(DateTime now);
}
