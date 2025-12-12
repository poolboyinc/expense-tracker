namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}