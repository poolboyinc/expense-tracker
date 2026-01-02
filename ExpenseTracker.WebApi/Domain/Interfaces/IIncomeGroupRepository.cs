using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IIncomeGroupRepository
{
    Task<IncomeGroup> CreateAsync(IncomeGroup group);
    Task<IncomeGroup?> GetByIdAsync(int id);
    Task<List<IncomeGroup>> GetAllByUserIdAsync(Guid userId);
    Task<IncomeGroup> UpdateAsync(IncomeGroup group);
    Task<bool> DeleteAsync(int id);
}