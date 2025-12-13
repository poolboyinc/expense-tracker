using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface ISavingsPlanRepository
{
    Task<SavingsPlan?> GetByIdAsync(int id, Guid userId);
    Task<List<SavingsPlan>> GetAllAsync(Guid userId);
    Task<SavingsPlan> CreateAsync(SavingsPlan plan);
    Task UpdateAsync(SavingsPlan plan);
    Task DeleteAsync(SavingsPlan plan);
}
