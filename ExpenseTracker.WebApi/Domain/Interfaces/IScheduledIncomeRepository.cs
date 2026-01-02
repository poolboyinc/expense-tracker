using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IScheduledIncomeRepository
{
    Task<List<ScheduledIncome>> GetDueAsync(DateTime now);
    Task CreateAsync(ScheduledIncome income);
    Task UpdateAsync(ScheduledIncome income);
}
