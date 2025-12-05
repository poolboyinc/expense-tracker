using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.ServiceContracts;

public interface IExpenseGroupService
{
    Task<ExpenseGroup> CreateGroupAsync(ExpenseGroup group, string userId);
    
    Task<ExpenseGroup?> GetGroupByIdAsync(int id, string userId);
    
    Task<List<ExpenseGroup>> GetAllGroupsForUserAsync(string userId);
    
    Task<ExpenseGroup> UpdateGroupAsync(ExpenseGroup group, string userId);
    
    Task<bool> DeleteGroupAsync(int id, string userId);
}