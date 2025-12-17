using ExpenseTracker.WebApi.Application.ServiceInterfaces;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

public class SummaryEmailWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<SummaryEmailWorker> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var service = scope.ServiceProvider
                    .GetRequiredService<ISummaryEmailService>();

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
            catch (Exception ex)
            {
                logger.LogError(ex, "SummaryEmailWorker failed");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
