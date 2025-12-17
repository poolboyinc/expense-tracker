using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Application.Services;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ExpenseTracker.UnitTests.Savings;

public class SavingsPlanServiceTests
{
    private readonly Mock<ISavingsPlanRepository> _repo = new();
    private readonly Mock<IIncomeRepository> _incomeRepo = new();
    private readonly Mock<IExpenseRepository> _expenseRepo = new();
    private readonly Mock<ISavingsPlanCalculator> _calculator = new();
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IUserServiceContext> _userContext = new();
    private readonly Mock<IEmailService> _emailServ = new();

    public SavingsPlanServiceTests()
    {
        _userContext.Setup(c => c.GetCurrentUserId()).Returns(Guid.NewGuid());
    }
    
    private SavingsPlanService Sut => new(
        _repo.Object,
        _calculator.Object,
        _userRepo.Object,
        _userContext.Object,
        _incomeRepo.Object,
        _expenseRepo.Object,
        _emailServ.Object
    );

    [Fact]
    public async Task ProcessMonthlyContributions_DeactivatesPlan_WhenNotEnoughMoney()
    {
        var testDate = new DateTime(2025, 1, 15);
        var userId = Guid.NewGuid();
        
        var plan = new SavingsPlan {
            UserId = userId,
            IsActive = true,
            Contributions = new List<SavingsPlanContribution> {
                new() { Year = 2025, Month = 1, PlannedAmount = 500, IsCompleted = false }
            }
        };

        _repo.Setup(r => r.GetAllActiveAsync()).ReturnsAsync(new List<SavingsPlan> { plan });
        
        _incomeRepo.Setup(r => r.GetTotalIncomeForMonthAsync(userId, 2025, 1)).ReturnsAsync(300);
        _expenseRepo.Setup(r => r.GetTotalForMonthAsync(userId, 2025, 1)).ReturnsAsync(100);
        
        await Sut.ProcessMonthlyContributionsAsync(testDate);
        
        plan.IsActive.Should().BeFalse();
    }
}