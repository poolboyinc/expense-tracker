using ExpenseTracker.IntegrationTests.Infrastructure;
using ExpenseTracker.WebApi.Application.DTOs.Expense;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Application.Services;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Infrastructure.Repositories;
using FluentAssertions;
using Moq;

namespace ExpenseTracker.IntegrationTests.Savings;

public class SavingsShieldTests : IntegrationTestBase
{
    private readonly ExpenseService _expenseService;

    public SavingsShieldTests()
    {
        var incomeRepo = new IncomeRepository(Context);
        var expenseRepo = new ExpenseRepository(Context);
        var savingsRepo = new SavingsPlanRepository(Context);
        var groupRepo = new ExpenseGroupRepository(Context);
        
        var userRepo = new UserRepository(Context); 
        
        var userContextMock = new Mock<IUserServiceContext>();
        userContextMock.Setup(x => x.GetCurrentUserId()).Returns(TestUserId);
        
        var emailMock = new Mock<IEmailService>();
        
        var shield = new SavingsAvailabilityService(incomeRepo, expenseRepo, savingsRepo);
        
        _expenseService = new ExpenseService(
            expenseRepo,         
            groupRepo,          
            userContextMock.Object, 
            userRepo,            
            shield,              
            emailMock.Object     
        );
    }

    [Fact]
    public async Task CreateExpense_ShouldFail_WhenViolatingSavingsLogic()
    {

        var user = new User { Id = TestUserId, Email = "test@test.com", IsPremium = true };
        Context.User.Add(user);
        
        Context.Income.Add(new Income { UserId = TestUserId, Amount = 5000, Date = DateTime.UtcNow });
        
        Context.SavingsPlan.Add(new SavingsPlan 
        { 
            UserId = TestUserId, 
            TargetAmount = 10000, 
            TargetDate = DateTime.UtcNow.AddMonths(6),
            IsActive = true 
        });
        
        await Context.SaveChangesAsync();
        
        var dto = new ExpenseCreateDto(
            4500, 
            "Too Expensive", 
            DateTime.UtcNow, 
            1
        );
        
        Func<Task> act = async () => await _expenseService.CreateExpenseAsync(dto);
        
        await act.Should().ThrowAsync<InvalidOperationException>();
        Context.Expense.Count().Should().Be(0);
    }

    [Fact]
    public async Task CreateExpense_ShouldSucceed_WhenFundsAreAvailable()
    {
        var user = new User { Id = TestUserId, Email = "test@test.com", IsPremium = true };
        Context.User.Add(user);
        
        var group = new ExpenseGroup { Id = 2, Name = "General", UserId = TestUserId };
        Context.ExpenseGroup.Add(group);
        
        Context.Income.Add(new Income { UserId = TestUserId, Amount = 5000, Date = DateTime.UtcNow });
    
        await Context.SaveChangesAsync();

        var dto = new ExpenseCreateDto(500, "Valid Expense", DateTime.UtcNow, 2);
        await _expenseService.CreateExpenseAsync(dto);

        Context.Expense.Should().Contain(e => e.Amount == 500);
    }
}