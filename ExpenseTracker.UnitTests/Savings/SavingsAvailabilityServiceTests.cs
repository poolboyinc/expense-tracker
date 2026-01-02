using ExpenseTracker.WebApi.Application.Services;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ExpenseTracker.UnitTests.Savings;
public class SavingsAvailabilityServiceTests
{
    private readonly Mock<IIncomeRepository> _incomeRepo = new();
    private readonly Mock<IExpenseRepository> _expenseRepo = new();
    private readonly Mock<ISavingsPlanRepository> _savingsRepo = new();
    private readonly Guid _userId = Guid.NewGuid();

    private SavingsAvailabilityService Sut => new(
        _incomeRepo.Object,
        _expenseRepo.Object,
        _savingsRepo.Object
    );

    [Fact]
    public async Task CanSpendAsync_ReturnsTrue_WhenSurplusIsSufficient()
    {
        SetupMockData(income: 5000, expenses: 2000, plannedSavings: 500, actualSavings: 0);

        var result = await Sut.CanSpendAsync(_userId, 1000m);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task CanSpendAsync_ReturnsFalse_WhenExpenseDipsIntoSavings()
    {
        SetupMockData(income: 3000, expenses: 2500, plannedSavings: 400, actualSavings: 0);
        
        var result = await Sut.CanSpendAsync(_userId, 150m);
        
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CanSpendAsync_AccountsForAlreadyPaidContributions()
    {
        SetupMockData(income: 3000, expenses: 2000, plannedSavings: 500, actualSavings: 500);

        var result = await Sut.CanSpendAsync(_userId, 800m);

        result.Should().BeTrue();
    }

    private void SetupMockData(decimal income, decimal expenses, decimal plannedSavings, decimal actualSavings)
    {
        var now = DateTime.UtcNow;
        _incomeRepo.Setup(r => r.GetTotalIncomeForMonthAsync(_userId, now.Year, now.Month))
            .ReturnsAsync(income);
        _expenseRepo.Setup(r => r.GetTotalForMonthAsync(_userId, now.Year, now.Month))
            .ReturnsAsync(expenses);

        var plan = new SavingsPlan
        {
            UserId = _userId,
            Contributions = new List<SavingsPlanContribution>
            {
                new() { Year = now.Year, Month = now.Month, PlannedAmount = plannedSavings, ActualAmount = actualSavings }
            }
        };

        _savingsRepo.Setup(r => r.GetAllAsync(_userId))
            .ReturnsAsync(new List<SavingsPlan> { plan });
    }
}