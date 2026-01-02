using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface ISavingsPlanCalculator
{
    IReadOnlyCollection<SavingsPlanContribution> CalculateContributions(
        decimal targetAmount,
        DateTime startDate,
        DateTime targetDate
    );
}

