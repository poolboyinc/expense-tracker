using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IExpenseGroupRepository
{
    Task<List<ExpenseGroup>> GetAllGroupsAsync(string userId);

    Task<ExpenseGroup?> GetGroupByIdAsync(int id, string userId);

    Task<ExpenseGroup> CreateGroupAsync(ExpenseGroup group);

    Task<ExpenseGroup> UpdateGroupAsync(ExpenseGroup group);

    Task DeleteGroupAsync(ExpenseGroup group);
    
    Task<bool> ExistsByNameAsync(string name, string userId);
}