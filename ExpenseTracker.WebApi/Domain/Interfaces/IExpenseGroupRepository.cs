using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IExpenseGroupRepository
{
    Task<ExpenseGroup> CreateGroupAsync(ExpenseGroup group);
    
    Task<ExpenseGroup?> GetGroupByIdAsync(int id);
    
    Task<List<ExpenseGroup>> GetGroupsByUserIdAsync(string userId);
    
    Task<ExpenseGroup> UpdateGroupAsync(ExpenseGroup group);
    
    Task<bool> DeleteGroupAsync(int id);
    
    Task<bool> GroupExistsAsync(int id);
    
}