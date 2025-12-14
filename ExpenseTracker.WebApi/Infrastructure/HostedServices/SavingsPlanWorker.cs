using ExpenseTracker.WebApi.Application.ServiceInterfaces;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

public class SavingsPlanWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<SavingsPlanWorker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessSavingsPlansAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }

    private async Task ProcessSavingsPlansAsync(CancellationToken token)
    {
        using var scope = scopeFactory.CreateScope();

        var savingsService = scope.ServiceProvider
            .GetRequiredService<ISavingsPlanService>();

        try
        {
            await savingsService.ProcessMonthlyContributionsAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while processing savings plans");
        }
    }
}
