using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

public class ScheduledIncomeWorker(
    IServiceProvider serviceProvider,
    ILogger<ScheduledIncomeWorker> logger,
    IOptions<ScheduledWorkerOptions> options)
    : BackgroundService
{
    private readonly TimeSpan _interval =
        TimeSpan.FromSeconds(options.Value.IntervalSeconds);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("ScheduledIncomeWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var svc = scope.ServiceProvider
                    .GetRequiredService<IScheduledIncomeService>();

                var created = await svc
                    .ProcessDueScheduledIncomesAsync(DateTime.UtcNow);

                if (created > 0)
                {
                    logger.LogInformation(
                        "ScheduledIncomeWorker created {Count} incomes.",
                        created);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ScheduledIncomeWorker error");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}

