using ExpenseTracker.WebApi.Application.Services;
using FluentAssertions;
using Xunit;

namespace ExpenseTracker.UnitTests.Savings;

public class SavingsPlanCalculatorTests
{
    [Fact]
    public void CalculateContributions_SplitsAmountAcrossMonths()
    {
        var calculator = new SavingsPlanCalculator();

        var targetAmount = 1200m;
        var start = new DateTime(2025, 1, 1);
        var target = new DateTime(2025, 7, 1);
        
        var contributions = calculator.CalculateContributions(
            targetAmount,
            start,
            target
        ).ToList();
        
        contributions.Should().HaveCount(7);
        contributions.Sum(c => c.PlannedAmount).Should().Be(targetAmount);
    }
    
    [Fact]
    public void CalculateContributions_OneMonthTarget_AssignsFullAmount()
    {
        var calculator = new SavingsPlanCalculator();

        var contributions = calculator.CalculateContributions(
            500m,
            new DateTime(2025, 1, 1),
            new DateTime(2025, 1, 31)
        ).ToList();

        contributions.Should().HaveCount(1);
        contributions[0].PlannedAmount.Should().Be(500m);
    }

    [Fact]
    public void CalculateContributions_Throws_WhenTargetBeforeStart()
    {
        var calculator = new SavingsPlanCalculator();

        Action act = () =>
            calculator.CalculateContributions(
                500m,
                new DateTime(2025, 5, 1),
                new DateTime(2025, 1, 1)
            ).ToList();

        act.Should().Throw<InvalidOperationException>();
    }

}

