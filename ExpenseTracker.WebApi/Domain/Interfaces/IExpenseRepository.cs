using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IExpenseRepository
{
    Task<Expense> GetByIdAsync(int id);
    Task AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(Expense expense);
    
    Task<ICollection<Expense>> GetExpensesAsync(
        int? groupId,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        string sortBy
        );
}