using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IExpenseService
{
    Task<List<Expense>> GetAllExpensesAsync(string userId); 
    
    Task<Expense?> GetExpenseByIdAsync(int id, string userId); 
    
    Task<Expense> CreateExpenseAsync(Expense expense, string userId);
    
    Task<ICollection<Expense>> GetFilteredExpensesAsync(
        string userId,
        int? groupId,
        string? searchTerm,
        int pageNumber,
        int pageSize);

    Task UpdateExpenseAsync(Expense expense, string userId);
    
    Task DeleteExpenseAsync(int id, string userId);
}