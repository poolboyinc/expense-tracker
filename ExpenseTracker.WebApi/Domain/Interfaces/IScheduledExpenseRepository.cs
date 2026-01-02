using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IScheduledExpenseRepository
{
    Task<ScheduledExpense> CreateAsync(ScheduledExpense item);
    Task<ScheduledExpense?> GetByIdAsync(int id);
    Task<List<ScheduledExpense>> GetAllForUserAsync(Guid userId);
    Task UpdateAsync(ScheduledExpense item);
    Task DeleteAsync(ScheduledExpense item);
    
    Task<List<ScheduledExpense>> GetDueScheduledExpensesAsync(DateTime utcNow);
}