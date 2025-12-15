using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

public class SavingsPlanWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<SavingsPlanWorker> logger,
    IOptions<SavingsPlanWorkerOptions> options)
    : BackgroundService
{
    private readonly TimeSpan _interval =
        TimeSpan.FromHours(options.Value.IntervalHours);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("SavingsPlanWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessSavingsPlansAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled error in SavingsPlanWorker");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task ProcessSavingsPlansAsync(CancellationToken token)
    {
        using var scope = scopeFactory.CreateScope();

        var savingsService = scope.ServiceProvider
            .GetRequiredService<ISavingsPlanService>();

        await savingsService.ProcessMonthlyContributionsAsync(DateTime.UtcNow);
    }
}
