using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(int id, Guid userId);
    Task AddAsync(Expense expense);
    Task UpdateAsync(Expense expense);
    Task DeleteAsync(Expense expense);
    Task<ExpenseGroup?> GetGroupByIdAsync(int groupId);

    Task<List<Expense>> GetExpensesAsync(
        Guid userId,
        int? groupId,
        string? searchTerm,
        int pageNumber,
        int pageSize
    );

    Task<int> CountExpensesInGroupAsync(int groupId, Guid userId);
}