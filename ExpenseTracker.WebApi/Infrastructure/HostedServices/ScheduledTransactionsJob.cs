using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using Quartz;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

[DisallowConcurrentExecution]
public class ScheduledTransactionsJob(IServiceScopeFactory scopeFactory) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = scopeFactory.CreateScope();
        var incomeSvc = scope.ServiceProvider.GetRequiredService<IScheduledIncomeService>();
        var expenseSvc = scope.ServiceProvider.GetRequiredService<IScheduledExpenseService>();

        await incomeSvc.ProcessDueScheduledIncomesAsync(DateTime.UtcNow);
        await expenseSvc.ProcessDueScheduledExpensesAsync(DateTime.UtcNow);
    }
}