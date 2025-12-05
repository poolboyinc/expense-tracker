using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.ServiceContracts;

public interface IIncomeService
{
    Task<Income> CreateIncomeAsync(Income income, string userId);
    
    Task<Income?> GetIncomeByIdAsync(int id, string userId);
    
    Task<List<Income>> GetAllIncomesForUserAsync(string userId);
    
    Task<Income> UpdateIncomeAsync(Income income, string userId);
    
    Task<bool> DeleteIncomeAsync(int id, string userId);
}