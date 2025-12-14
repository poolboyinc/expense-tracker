using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IIncomeRepository
{
    Task<Income> CreateIncomeAsync(Income income);

    Task<Income?> GetIncomeByIdAsync(int id);

    Task<List<Income>> GetAllIncomesByUserIdAsync(Guid userId);

    Task<Income> UpdateIncomeAsync(Income income);

    Task<bool> DeleteIncomeAsync(int id);
    
    Task<decimal> GetAverageMonthlyIncomeAsync(Guid userId);
    
    Task<decimal> GetTotalIncomeForMonthAsync(Guid userId, int year, int month);


}