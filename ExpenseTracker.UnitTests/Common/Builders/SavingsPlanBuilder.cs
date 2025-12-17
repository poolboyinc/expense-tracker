using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.UnitTests.Common.Builders;

public class SavingsPlanBuilder
{
    private SavingsPlan _plan = new()
    {
        UserId = Guid.NewGuid(),
        TargetAmount = 1000,
        IsActive = true
    };

    public SavingsPlanBuilder WithAmount(decimal amount) {
        _plan.TargetAmount = amount;
        return this;
    }

    public SavingsPlanBuilder Deactivated() {
        _plan.IsActive = false;
        return this;
    }

    public SavingsPlan Build() => _plan;
}