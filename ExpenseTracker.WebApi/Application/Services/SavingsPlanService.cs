using ExpenseTracker.WebApi.Application.DTOs.Savings;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;
public class SavingsPlanService(
    ISavingsPlanRepository repository,
    ISavingsPlanCalculator calculator,
    IUserRepository userRepository,
    IUserServiceContext userServiceContext,
    IIncomeRepository incomeRepository,
    IExpenseRepository expenseRepository,
    IEmailService emailService
    )
    : ISavingsPlanService
{
    public async Task<SavingsPlanDto> CreateAsync(SavingsPlanCreateDto dto)
    {
        var userId = userServiceContext.GetCurrentUserId();
        var user = await userRepository.GetUserById(userId);
        
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        if (!user.IsPremium)
        {
            throw new InvalidOperationException("Savings plans are available to premium users only.");
        }

        var contributions = calculator.CalculateContributions(
            dto.TargetAmount,
            DateTime.UtcNow,
            dto.TargetDate
        );

        var plan = new SavingsPlan
        {
            UserId = userId,
            TargetAmount = dto.TargetAmount,
            TargetDate = dto.TargetDate,
            Contributions = contributions.ToList()
        };

        await repository.CreateAsync(plan);

        return plan.ToDto();
    }

    public async Task<IReadOnlyCollection<SavingsPlanDto>> GetAllAsync()
    {
        var userId = userServiceContext.GetCurrentUserId();
        var plans = await repository.GetAllAsync(userId);
        return plans.Select(p => p.ToDto()).ToList();
    }

    public async Task<SavingsPlanDto?> GetByIdAsync(int id)
    {
        var userId = userServiceContext.GetCurrentUserId();
        var plan = await repository.GetByIdAsync(id, userId);
        return plan?.ToDto();
    }

    public async Task DeleteAsync(int id)
    {
        var userId = userServiceContext.GetCurrentUserId();
        
        var plan = await repository.GetByIdAsync(id, userId);
        if (plan == null)
        {
            throw new KeyNotFoundException();
        }

        await repository.DeleteAsync(plan);
    }
    
    public async Task ProcessMonthlyContributionsAsync(DateTime now)
    {
        var year = now.Year;
        var month = now.Month;

        var plans = await repository.GetAllActiveAsync();

        foreach (var plan in plans)
        {
            await ProcessPlanForMonthAsync(plan, year, month);
        }
    }
    
    private async Task ProcessPlanForMonthAsync(
        SavingsPlan plan,
        int year,
        int month)
    {
        var contribution = plan.Contributions
            .FirstOrDefault(c => c.Year == year && c.Month == month);

        if (contribution == null || contribution.IsCompleted)
        {
            return;
        }

        var income = await incomeRepository
            .GetTotalIncomeForMonthAsync(plan.UserId, year, month);

        var expenses = await expenseRepository
            .GetTotalForMonthAsync(plan.UserId, year, month);

        var available = income - expenses;

        if (available < contribution.PlannedAmount)
        {
            plan.IsActive = false;
            await repository.UpdateAsync(plan);
            return;
        }

        contribution.ActualAmount = contribution.PlannedAmount;
        contribution.IsCompleted = true;

        var totalSaved = plan.Contributions
            .Where(c => c.IsCompleted)
            .Sum(c => c.ActualAmount);

        if (totalSaved >= plan.TargetAmount)
        {
            plan.IsActive = false;
            
            if (!plan.GoalReachedNotified && plan.User.IsPremium)
            {
                await emailService.SendEmailAsync(
                    plan.User.Email,
                    " Savings goal achieved!",
                    $"""
                     <h2>Congratulations!</h2>
                     <p>You’ve successfully reached your savings goal of <strong>{plan.TargetAmount:C}</strong>.</p>
                     <p>Target date: {plan.TargetDate:MMMM yyyy}</p>
                     """
                );

                plan.GoalReachedNotified = true;
            }
        }

        await repository.UpdateAsync(plan);
    }


}
