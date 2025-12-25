namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface ISummaryEmailService
{
    Task SendWeeklySummariesAsync(DateTime now);
    Task SendMonthlySummariesAsync(DateTime now);
}