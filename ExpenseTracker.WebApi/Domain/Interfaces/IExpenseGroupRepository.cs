using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IExpenseGroupRepository
{
    Task<List<ExpenseGroup>> GetAllGroupsAsync(Guid userId);

    Task<ExpenseGroup?> GetGroupByIdAsync(int id, Guid userId);

    Task<ExpenseGroup> CreateGroupAsync(ExpenseGroup group);

    Task<ExpenseGroup> UpdateGroupAsync(ExpenseGroup group);

    Task DeleteGroupAsync(ExpenseGroup group);

    Task<bool> ExistsByNameAsync(string name, Guid userId);
}