using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.ServiceContracts;

public interface IExpenseService
{
    Task<Expense> CreateExpenseAsync(Expense expense, string userId);
    
    Task<ICollection<Expense>> GetFilteredExpensesAsync(
        string userId,
        int? groupId,
        string? searchTerm,
        int pageNumber,
        int pageSize);

    Task<Expense?> GetExpenseByIdAsync(int id, string userId);

    Task UpdateExpenseAsync(Expense expense, string userId);
    
    Task DeleteExpenseAsync(int id, string userId);
}