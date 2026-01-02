using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using Quartz;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

public class SummaryEmailJob(IServiceScopeFactory scopeFactory) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        using var scope = scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ISummaryEmailService>();
        var now = DateTime.UtcNow;

        if (now.DayOfWeek == DayOfWeek.Monday)
        {
            await service.SendWeeklySummariesAsync(now);
        }

        if (now.Day == 1)
        {
            await service.SendMonthlySummariesAsync(now);
        }
    }
}