using ExpenseTracker.WebApi.Application.Services;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Enums;
using FluentAssertions;

namespace ExpenseTracker.UnitTests.Workers;

public class RecurrenceCalculatorTests
{
    [Fact]
    public void ComputeNextRun_Daily_ReturnsNextDay()
    {
        var currentRun = new DateTime(2025, 1, 1);
        var scheduledItem = new ScheduledExpense 
        { 
            Frequency = RecurrenceFrequency.Daily, 
            NextRunAt = currentRun 
        };

        var next = RecurrenceCalculator.ComputeNextRun(scheduledItem);

        next.Should().Be(currentRun.AddDays(1));
    }

    [Fact]
    public void ComputeNextRun_Monthly_ReturnsNextMonth()
    {
        var currentRun = new DateTime(2025, 1, 31); 
        var scheduledItem = new ScheduledExpense 
        { 
            Frequency = RecurrenceFrequency.Monthly, 
            NextRunAt = currentRun 
        };

        var next = RecurrenceCalculator.ComputeNextRun(scheduledItem);
        
        next.Should().Be(new DateTime(2025, 2, 28));
    }
    
    [Fact]
    public void ComputeNextRun_MonthlyOn31st_InFebruary_ReturnsLastDayOfFeb()
    {
        var item = new ScheduledExpense 
        { 
            Frequency = RecurrenceFrequency.Monthly, 
            NextRunAt = new DateTime(2025, 1, 31) 
        };

        var next = RecurrenceCalculator.ComputeNextRun(item);

        next.Should().Be(new DateTime(2025, 2, 28));
    }
}