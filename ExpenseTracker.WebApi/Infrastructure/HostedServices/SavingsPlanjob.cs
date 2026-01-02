using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using Quartz;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

[DisallowConcurrentExecution]
public class SavingsPlanJob(IServiceScopeFactory scopeFactory, ILogger<SavingsPlanJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Processing Monthly Savings Contributions...");
        using var scope = scopeFactory.CreateScope();
        var savingsService = scope.ServiceProvider.GetRequiredService<ISavingsPlanService>();
        await savingsService.ProcessMonthlyContributionsAsync(DateTime.UtcNow);
    }
}