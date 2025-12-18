using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.UnitTests.Common.Builders;

public class SavingsPlanBuilder
{
    private readonly SavingsPlan _plan = new()
    {
        TargetAmount = 1000m,
        TargetDate = DateTime.UtcNow.AddMonths(6),
        IsActive = true,
        GoalReachedNotified = false,
        Contributions = new List<SavingsPlanContribution>()
    };

    public SavingsPlanBuilder WithUser(Guid userId, bool isPremium = true)
    {
        _plan.UserId = userId;
        _plan.User = new User
        {
            Id = userId,
            Email = "test@test.com",
            IsPremium = isPremium
        };
        return this;
    }

    public SavingsPlanBuilder WithTarget(decimal amount)
    {
        _plan.TargetAmount = amount;
        return this;
    }

    public SavingsPlanBuilder WithContribution(int year, int month, decimal plannedAmount)
    {
        _plan.Contributions.Add(new SavingsPlanContribution
        {
            Year = year,
            Month = month,
            PlannedAmount = plannedAmount,
            ActualAmount = 0,
            IsCompleted = false
        });
        return this;
    }
    
    public SavingsPlanBuilder WithCompletedContribution(int year, int month, decimal amount)
    {
        _plan.Contributions.Add(new SavingsPlanContribution
        {
            Year = year,
            Month = month,
            PlannedAmount = amount,
            ActualAmount = amount,
            IsCompleted = true
        });
        return this;
    }
    
    public SavingsPlanBuilder Inactive()
    {
        _plan.IsActive = false;
        return this;
    }

    public SavingsPlan Build() => _plan;
}