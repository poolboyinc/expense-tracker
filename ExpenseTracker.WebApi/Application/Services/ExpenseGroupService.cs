using ExpenseTracker.WebApi.Application.ServiceContracts;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class ExpenseGroupService : IExpenseGroupService
{
    private readonly IExpenseGroupRepository _groupRepository;
    private readonly IExpenseRepository _expenseRepository; 

    public ExpenseGroupService(IExpenseGroupRepository groupRepository, IExpenseRepository expenseRepository)
    {
        _groupRepository = groupRepository;
        _expenseRepository = expenseRepository;
    }
    
    private async Task<ExpenseGroup?> GetAuthorizedGroupAsync(int id, string userId)
    {
        var group = await _groupRepository.GetGroupByIdAsync(id);
        
        if (group == null)
            return null;

        if (group.UserId != userId)
        {
            return null; 
        }

        return group;
    }
    
    public async Task<ExpenseGroup> CreateGroupAsync(ExpenseGroup group, string userId)
    {
        group.UserId = userId;
        
        var existingGroups = await _groupRepository.GetGroupsByUserIdAsync(userId);
        if (existingGroups.Any(g => g.Name.Equals(group.Name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Expense group with name '{group.Name}' already exists for this user.");
        }

        return await _groupRepository.CreateGroupAsync(group);
    }
    
    public Task<ExpenseGroup?> GetGroupByIdAsync(int id, string userId)
    {
        return GetAuthorizedGroupAsync(id, userId);
    }
    
    public Task<List<ExpenseGroup>> GetAllGroupsForUserAsync(string userId)
    {
        return _groupRepository.GetGroupsByUserIdAsync(userId);
    }
    
    public async Task<ExpenseGroup> UpdateGroupAsync(ExpenseGroup group, string userId)
    {
        var existingGroup = await GetAuthorizedGroupAsync(group.Id, userId);
        if (existingGroup == null)
        {
            throw new KeyNotFoundException($"Expense Group with this ID not found or unauthorized.");
        }
        
        var existingGroups = await _groupRepository.GetGroupsByUserIdAsync(userId);
        if (existingGroups.Any(g => g.Id != group.Id && g.Name.Equals(group.Name, StringComparison.OrdinalIgnoreCase)))
        {
             throw new InvalidOperationException($"Another group with this name  already exists for this user.");
        }
        
        group.UserId = userId; 
        
        return await _groupRepository.UpdateGroupAsync(group);
    }
    
    public async Task<bool> DeleteGroupAsync(int id, string userId)
    {
        var groupToDelete = await GetAuthorizedGroupAsync(id, userId);
        
        if (groupToDelete == null)
        {
            return false;
        }
        
        //cascade deletion in ef core
        
        var expensesCount = await _expenseRepository.CountExpensesInGroupAsync(id, userId);
        
        if (expensesCount > 0)
        {
            throw new InvalidOperationException($"Cannot delete group.");
        }

        return await _groupRepository.DeleteGroupAsync(id);
    }
}