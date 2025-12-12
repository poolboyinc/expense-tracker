using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

public class ScheduledExpenseWorker(
    IServiceProvider serviceProvider,
    ILogger<ScheduledExpenseWorker> logger,
    IOptions<ScheduledWorkerOptions> options)
    : BackgroundService
{
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(options.Value.IntervalSeconds);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("ScheduledExpenseWorker started.");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var svc = scope.ServiceProvider.GetRequiredService<IScheduledExpenseService>();
                var created = await svc.ProcessDueScheduledExpensesAsync(DateTime.UtcNow);
                if (created > 0)
                {
                    logger.LogInformation("ScheduledExpenseWorker created {Count} expenses.", created);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ScheduledExpenseWorker error");
            }
            await Task.Delay(_interval, stoppingToken);
        }
    }
}
