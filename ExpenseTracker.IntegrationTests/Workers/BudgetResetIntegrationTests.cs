using ExpenseTracker.IntegrationTests.Infrastructure;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Infrastructure.Repositories;
using FluentAssertions;

namespace ExpenseTracker.IntegrationTests.Workers;

public class BudgetResetIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task ResetBudgetCapNotifications_ShouldClearFlagsForAllGroups()
    {
        var group = new ExpenseGroup 
        { 
            UserId = TestUserId, 
            Name = "Food", 
            BudgetCapNotified = true
        };
        Context.ExpenseGroup.Add(group);
        await Context.SaveChangesAsync();

        var repo = new ExpenseGroupRepository(Context);
        
        await repo.ResetBudgetCapNotificationsAsync();
        
        var updatedGroup = await Context.ExpenseGroup.FindAsync(group.Id);
        updatedGroup!.BudgetCapNotified.Should().BeFalse(); 
    }
}