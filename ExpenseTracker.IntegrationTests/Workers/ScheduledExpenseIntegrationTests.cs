using ExpenseTracker.IntegrationTests.Infrastructure;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Application.Services;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Enums;
using ExpenseTracker.WebApi.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExpenseTracker.IntegrationTests.Workers;
public class ScheduledExpenseIntegrationTests : IntegrationTestBase
{
    private readonly ScheduledExpenseService _sut;

    public ScheduledExpenseIntegrationTests()
    {
        var scheduledRepo = new ScheduledExpenseRepository(Context);
        var expenseRepo = new ExpenseRepository(Context);
        var groupRepo = new ExpenseGroupRepository(Context);
        
        var userContextMock = new Mock<IUserServiceContext>();
        userContextMock.Setup(x => x.GetCurrentUserId()).Returns(TestUserId);
        
        _sut = new ScheduledExpenseService(
            scheduledRepo, 
            expenseRepo, 
            groupRepo, 
            userContextMock.Object
        );
    }

    [Fact]
    public async Task ProcessDueScheduledExpenses_ShouldReturnCountOfProcessedItems()
    {
        var user = new User { Id = TestUserId, Email = "worker@test.com" };
        Context.User.Add(user);

        var group = new ExpenseGroup { Id = 1, Name = "Bills", UserId = TestUserId };
        Context.ExpenseGroup.Add(group);
        
        var executionTime = new DateTime(2025, 12, 20, 10, 0, 0, DateTimeKind.Utc);
        
        var scheduled = new ScheduledExpense 
        { 
            UserId = TestUserId,
            Amount = 100.00m,
            Description = "Rent",
            Frequency = RecurrenceFrequency.Monthly,
            NextRunAt = executionTime, 
            IsActive = true,
            ExpenseGroupId = 1
        };
        Context.ScheduledExpense.Add(scheduled);
        await Context.SaveChangesAsync();
        
        var simulatedNow = executionTime.AddHours(1);
        int processedCount = await _sut.ProcessDueScheduledExpensesAsync(simulatedNow);
        
        processedCount.Should().Be(1);
        
        var exists = await Context.Expense.AnyAsync(e => e.Description == "Rent");
        exists.Should().BeTrue();
        
        var updated = await Context.ScheduledExpense.FindAsync(scheduled.Id);
        updated!.NextRunAt.Should().BeAfter(executionTime);
    }
}