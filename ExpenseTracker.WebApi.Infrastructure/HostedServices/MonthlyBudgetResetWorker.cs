using ExpenseTracker.WebApi.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

public class MonthlyBudgetResetWorker(IServiceProvider services) : BackgroundService
{
    private DateTime _nextRunTime = GetNextMonthStartUtc();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;

            if (now >= _nextRunTime)
            {
                using var scope = services.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IExpenseGroupRepository>();

                await repo.ResetBudgetCapNotificationsAsync();

                _nextRunTime = GetNextMonthStartUtc();
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }

    private static DateTime GetNextMonthStartUtc()
    {
        var now = DateTime.UtcNow;
        var nextMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(1);
        return nextMonth;
    }
}
