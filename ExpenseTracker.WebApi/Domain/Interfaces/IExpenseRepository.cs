using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(int id, string userId);
    Task AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(Expense expense);
    Task<ExpenseGroup?> GetGroupByIdAsync(int groupId);
    
    Task<List<Expense>> GetExpensesAsync(
        string userId,
        int? groupId,
        string? searchTerm,
        int pageNumber,
        int pageSize
        );
    
    Task<int> CountExpensesInGroupAsync(int groupId, string userId);
}