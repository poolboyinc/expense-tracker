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
    IUserServiceContext userServiceContext
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
}
