using ExpenseTracker.WebApi.Application.ServiceInterfaces;

namespace ExpenseTracker.WebApi.Infrastructure.HostedServices;

public class ScheduledExpenseWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ScheduledExpenseWorker> _logger;
    private readonly TimeSpan _interval;

    public ScheduledExpenseWorker(IServiceProvider serviceProvider, ILogger<ScheduledExpenseWorker> logger, IConfiguration config)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _interval = TimeSpan.FromMinutes(5); 
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ScheduledExpenseWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var svc = scope.ServiceProvider.GetRequiredService<IScheduledExpenseService>();
                var created = await svc.ProcessDueScheduledExpensesAsync(DateTime.UtcNow);
                if (created > 0)
                {
                    _logger.LogInformation("ScheduledExpenseWorker created {Count} expenses.", created);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ScheduledExpenseWorker error");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
