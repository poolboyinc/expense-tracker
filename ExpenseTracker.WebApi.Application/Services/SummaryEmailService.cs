using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class SummaryEmailService(
    IUserRepository userRepository,
    IIncomeRepository incomeRepository,
    IExpenseRepository expenseRepository,
    IEmailService emailService
) : ISummaryEmailService
{
    public async Task SendWeeklySummariesAsync(DateTime now)
    {
        var from = now.Date.AddDays(-7);
        var to = now.Date;

        var users = await GetPremiumUsersAsync();

        foreach (var user in users)
        {
            var income = await incomeRepository
                .GetTotalIncomeForRangeAsync(user.Id, from, to);

            var expenses = await expenseRepository
                .GetTotalForRangeAsync(user.Id, from, to);

            await SendSummaryEmail(
                user.Email,
                " Weekly Financial Summary",
                from,
                to,
                income,
                expenses
            );
        }
    }

    public async Task SendMonthlySummariesAsync(DateTime now)
    {
        var year = now.Year;
        var month = now.Month;

        var users = await GetPremiumUsersAsync();

        foreach (var user in users)
        {
            var income = await incomeRepository
                .GetTotalIncomeForMonthAsync(user.Id, year, month);

            var expenses = await expenseRepository
                .GetTotalForMonthAsync(user.Id, year, month);

            await SendSummaryEmail(
                user.Email,
                $" Monthly Summary – {now:MMMM yyyy}",
                new DateTime(year, month, 1),
                now,
                income,
                expenses
            );
        }
    }

    private async Task<List<User>> GetPremiumUsersAsync()
    {
        return (await userRepository.GetAllAsync())
            .Where(u => u.IsPremium)
            .ToList();
    }

    private async Task SendSummaryEmail(
        string email,
        string subject,
        DateTime from,
        DateTime to,
        decimal income,
        decimal expenses)
    {
        var net = income - expenses;

        var body = $"""
        <h2>{subject}</h2>
        <p><strong>Period:</strong> {from:dd MMM} – {to:dd MMM}</p>
        <ul>
            <li>Income: <strong>{income:C}</strong></li>
            <li>Expenses: <strong>{expenses:C}</strong></li>
            <li>Net: <strong>{net:C}</strong></li>
        </ul>
        """;

        await emailService.SendEmailAsync(email, subject, body);
    }
}
