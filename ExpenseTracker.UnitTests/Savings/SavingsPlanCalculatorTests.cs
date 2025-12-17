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
}

