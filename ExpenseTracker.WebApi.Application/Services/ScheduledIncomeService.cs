using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class ScheduledIncomeService(
    IScheduledIncomeRepository scheduledIncomeRepository,
    IIncomeRepository incomeRepository)
    : IScheduledIncomeService
{
    public async Task<int> ProcessDueScheduledIncomesAsync(DateTime now)
    {
        var due = await scheduledIncomeRepository.GetDueAsync(now);
        var created = 0;

        foreach (var s in due)
        {
            var income = new Income
            {
                Amount = s.Amount,
                Description = s.Description,
                Date = s.NextRunAt,
                IncomeGroupId = s.IncomeGroupId,
                UserId = s.UserId
            };

            await incomeRepository.CreateIncomeAsync(income);
            created++;

            var nextRun = RecurrenceCalculator.ComputeNextRun(s);

            if (nextRun == null || (s.EndAt != null && nextRun > s.EndAt))
            {
                s.IsActive = false;
            }
            else
            {
                s.NextRunAt = nextRun.Value;
            }

            await scheduledIncomeRepository.UpdateAsync(s);
        }

        return created;
    }
}
