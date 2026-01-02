using ExpenseTracker.IntegrationTests.Infrastructure;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Application.Services;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace ExpenseTracker.IntegrationTests.Workers;

public class SavingsPlanIntegrationTests : IntegrationTestBase
{
    private readonly SavingsPlanService _sut;

    public SavingsPlanIntegrationTests()
    {
        var savingsRepo = new SavingsPlanRepository(Context);
        var userRepo = new UserRepository(Context);
        var incomeRepo = new IncomeRepository(Context);
        var expenseRepo = new ExpenseRepository(Context);
        
        var userContextMock = new Mock<IUserServiceContext>();
        userContextMock.Setup(x => x.GetCurrentUserId()).Returns(TestUserId);
        
        Mock<IEmailService> emailMock = new();

        var calculator = new SavingsPlanCalculator(); 
        
        _sut = new SavingsPlanService(
            savingsRepo,
            calculator,
            userRepo,
            userContextMock.Object,
            incomeRepo,
            expenseRepo,
            emailMock.Object
        );
    }

    [Fact]
    public async Task ProcessMonthlyContributions_ShouldCompleteRecord_WhenSurplusIsSufficient()
    {
        var testDate = new DateTime(2025, 12, 1, 10, 0, 0, DateTimeKind.Utc);
        var year = testDate.Year;
        var month = testDate.Month;

        var user = new User { Id = TestUserId, Email = "saver@test.com", IsPremium = true };
        Context.User.Add(user);

        Context.Income.Add(new Income 
        { 
            UserId = TestUserId, 
            Amount = 5000, 
            Date = testDate, 
            IncomeGroupId = 1 
        });

        var plan = new SavingsPlan 
        { 
            UserId = TestUserId, 
            TargetAmount = 10000, 
            TargetDate = testDate.AddMonths(10),
            IsActive = true,
            CreatedAt = testDate.AddDays(-1)
        };

        var plannedContribution = new SavingsPlanContribution
        {
            SavingsPlan = plan,
            Year = year,
            Month = month,
            PlannedAmount = 1000, 
            ActualAmount = 0,   
            IsCompleted = false
        };
    
        Context.SavingsPlan.Add(plan);
        Context.SavingsPlanContribution.Add(plannedContribution);
        await Context.SaveChangesAsync();

        await _sut.ProcessMonthlyContributionsAsync(testDate);

        var result = await Context.SavingsPlanContribution
            .FirstOrDefaultAsync(c => c.SavingsPlanId == plan.Id && c.Year == year && c.Month == month);
    
        result.Should().NotBeNull("the service should have found the planned record and updated it");
        result!.IsCompleted.Should().BeTrue("because income (5000) > planned (1000)");
        result!.ActualAmount.Should().Be(1000);
    }
}