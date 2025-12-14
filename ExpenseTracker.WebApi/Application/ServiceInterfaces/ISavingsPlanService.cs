using ExpenseTracker.WebApi.Application.DTOs.Savings;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface ISavingsPlanService
{
    Task<SavingsPlanDto> CreateAsync( SavingsPlanCreateDto dto);
    Task<IReadOnlyCollection<SavingsPlanDto>> GetAllAsync();
    Task<SavingsPlanDto?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task ProcessMonthlyContributionsAsync();
}
