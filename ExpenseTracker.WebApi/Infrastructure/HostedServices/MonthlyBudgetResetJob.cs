using ExpenseTracker.WebApi.Domain.Interfaces;
using Quartz;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

[DisallowConcurrentExecution]
public class MonthlyBudgetResetJob(IServiceScopeFactory scopeFactory) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IExpenseGroupRepository>();
        await repo.ResetBudgetCapNotificationsAsync();
    }
}
