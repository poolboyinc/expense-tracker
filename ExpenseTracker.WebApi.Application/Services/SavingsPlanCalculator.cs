using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.Services;

public class SavingsPlanCalculator : ISavingsPlanCalculator
{
    public IReadOnlyCollection<SavingsPlanContribution> CalculateContributions(
        decimal targetAmount,
        DateTime startDate,
        DateTime targetDate)
    {
        if (targetDate <= startDate)
        {
            throw new InvalidOperationException("Target date must be in the future.");
        }

        var months = GetMonthCount(startDate, targetDate);
        if (months <= 0)
        {
            throw new InvalidOperationException("Savings period is invalid.");
        }

        var baseAmount = Math.Floor(targetAmount / months * 100) / 100;
        var remainder = targetAmount - (baseAmount * months);

        var contributions = new List<SavingsPlanContribution>();

        var current = new DateTime(startDate.Year, startDate.Month, 1);

        for (var i = 0; i < months; i++)
        {
            var amount = baseAmount;

            if (remainder > 0)
            {
                var extra = Math.Min(remainder, 0.01m);
                amount += extra;
                remainder -= extra;
            }

            contributions.Add(new SavingsPlanContribution
            {
                Year = current.Year,
                Month = current.Month,
                PlannedAmount = amount,
                ActualAmount = 0,
                IsCompleted = false
            });

            current = current.AddMonths(1);
        }

        return contributions;
    }

    private static int GetMonthCount(DateTime from, DateTime to)
    {
        return ((to.Year - from.Year) * 12) + to.Month - from.Month + 1;
    }
}
