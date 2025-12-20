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

public class ScheduledIncomeIntegrationTests : IntegrationTestBase
{
    private readonly ScheduledIncomeService _sut;

    public ScheduledIncomeIntegrationTests()
    {
        _sut = new ScheduledIncomeService(
            new ScheduledIncomeRepository(Context),
            new IncomeRepository(Context)
        );
    }

    [Fact]
    public async Task ProcessDueScheduledIncomes_ShouldCreateIncome_AndAdvanceDate()
    {
        var executionTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var scheduled = new ScheduledIncome 
        { 
            UserId = TestUserId, 
            Amount = 3000, 
            Description = "Salary", 
            NextRunAt = executionTime,
            Frequency = RecurrenceFrequency.Monthly,
            IncomeGroupId = 1 
        };
        Context.ScheduledIncome.Add(scheduled);
        await Context.SaveChangesAsync();
        
        var result = await _sut.ProcessDueScheduledIncomesAsync(executionTime.AddHours(1));
        
        result.Should().Be(1);
        var incomeExists = await Context.Income.AnyAsync(i => i.Description == "Salary");
        incomeExists.Should().BeTrue();
    }
}