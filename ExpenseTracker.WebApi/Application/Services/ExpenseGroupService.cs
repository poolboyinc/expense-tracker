using ExpenseTracker.WebApi.Application.ServiceInterfaces;
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
    
    public async Task<ExpenseGroup> CreateGroupAsync(ExpenseGroup group, string userId)
    {
        group.UserId = userId;
        
        var existingGroups = await _groupRepository.GetAllGroupsAsync(userId);
        if (existingGroups.Any(g => g.Name.Equals(group.Name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"Expense group with name '{group.Name}' already exists for this user.");
        }

        return await _groupRepository.CreateGroupAsync(group);
    }
    
    public Task<ExpenseGroup?> GetGroupByIdAsync(int id, string userId)
    {
        return _groupRepository.GetGroupByIdAsync(id, userId);
    }
    
    public Task<List<ExpenseGroup>> GetAllGroupsForUserAsync(string userId)
    {
        return _groupRepository.GetAllGroupsAsync(userId);
    }
    
    public async Task<ExpenseGroup> UpdateGroupAsync(ExpenseGroup group, string userId)
    {
        var existingGroup = await GetGroupByIdAsync(group.Id, userId);
        if (existingGroup == null)
        {
            throw new KeyNotFoundException($"Expense Group with this ID not found or unauthorized.");
        }
        
        existingGroup.Name = group.Name;
        existingGroup.MonthlyLimit = group.MonthlyLimit;
        
        return await _groupRepository.UpdateGroupAsync(group);
    }
    
    public async Task<bool> DeleteGroupAsync(int id, string userId)
    {
        var groupToDelete = await GetGroupByIdAsync(id, userId);
        
        if (groupToDelete == null)
        {
            return false;
        }
        
        var expensesCount = await _expenseRepository.CountExpensesInGroupAsync(id, userId);
        
        if (expensesCount > 0)
        {
            throw new InvalidOperationException($"Cannot delete group.");
        }

        await _groupRepository.DeleteGroupAsync(groupToDelete);

        return true;
    }
}