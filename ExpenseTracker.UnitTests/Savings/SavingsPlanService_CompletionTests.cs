using ExpenseTracker.UnitTests.Common.Builders;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Application.Services;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExpenseTracker.UnitTests.Savings;

public class SavingsPlanService_CompletionTests
{
    private readonly Mock<ISavingsPlanRepository> _repo = new();
    private readonly Mock<IIncomeRepository> _incomeRepo = new();
    private readonly Mock<IExpenseRepository> _expenseRepo = new();
    private readonly Mock<ISavingsPlanCalculator> _calculator = new();
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IUserServiceContext> _userContext = new();
    private readonly Mock<IEmailService> _email = new();

    private SavingsPlanService Sut => new(
        _repo.Object,
        _calculator.Object,
        _userRepo.Object,
        _userContext.Object,
        _incomeRepo.Object,
        _expenseRepo.Object,
        _email.Object
    );

    [Fact]
    public async Task CompletesContribution_WhenEnoughMoney()
    {
        var date = new DateTime(2025, 1, 10);
        var userId = Guid.NewGuid();

        var plan = new SavingsPlanBuilder()
            .WithUser(userId)
            .WithContribution(2025, 1, 200)
            .Build();

        _repo.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(new[] { plan }.ToList);
        _incomeRepo.Setup(r => r.GetTotalIncomeForMonthAsync(userId, 2025, 1)).ReturnsAsync(1000);
        _expenseRepo.Setup(r => r.GetTotalForMonthAsync(userId, 2025, 1)).ReturnsAsync(300);

        await Sut.ProcessMonthlyContributionsAsync(date);

        plan.Contributions.Single().IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task SkipsContribution_WhenAlreadyCompleted()
    {
        var date = new DateTime(2025, 1, 10);
        var userId = Guid.NewGuid();

        var plan = new SavingsPlanBuilder()
            .WithUser(userId)
            .WithCompletedContribution(2025, 1, 200)
            .Build();

        _repo.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(new[] { plan }.ToList());

        await Sut.ProcessMonthlyContributionsAsync(date);

        plan.Contributions.Single().ActualAmount.Should().Be(200);
    }

    [Fact]
    public async Task DoesNothing_WhenNoContributionForMonth()
    {
        var date = new DateTime(2025, 2, 10);
        var userId = Guid.NewGuid();

        var plan = new SavingsPlanBuilder()
            .WithUser(userId)
            .WithContribution(2025, 1, 200)
            .Build();

        _repo.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(new[] { plan }.ToList);

        await Sut.ProcessMonthlyContributionsAsync(date);

        plan.Contributions.Single().IsCompleted.Should().BeFalse();
    }
    
    [Fact]
    public async Task ProcessMonthlyContributions_SendsEmail_WhenGoalReached_AndPremium()
    {
        var date = new DateTime(2025, 1, 15);
        var userId = Guid.NewGuid();

        var plan = new SavingsPlanBuilder()
            .WithUser(userId, isPremium: true)
            .WithTarget(500)
            .WithContribution(2025, 1, 500)
            .Build();

        _repo.Setup(r => r.GetAllActiveAsync())
            .ReturnsAsync(new List<SavingsPlan> { plan });

        _incomeRepo.Setup(r => r.GetTotalIncomeForMonthAsync(userId, 2025, 1))
            .ReturnsAsync(1000);

        _expenseRepo.Setup(r => r.GetTotalForMonthAsync(userId, 2025, 1))
            .ReturnsAsync(0);

        await Sut.ProcessMonthlyContributionsAsync(date);

        _email.Verify(
            e => e.SendEmailAsync(
                plan.User.Email,
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Once);
    }
}
