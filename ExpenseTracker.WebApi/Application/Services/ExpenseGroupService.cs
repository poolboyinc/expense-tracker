using ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class ExpenseGroupService(
    IExpenseGroupRepository groupRepository,
    IExpenseRepository expenseRepository,
    IUserServiceContext userServiceContext)
    : IExpenseGroupService
{
    public async Task<ExpenseGroupDetailsDto> CreateGroupAsync(ExpenseGroupCreateDto dto)
    {
        var userId = userServiceContext.GetCurrentUserId();
        var group = dto.ToEntity(userId);

        var existingGroups = await groupRepository.GetAllGroupsAsync(userId);
        if (existingGroups.Any(g => g.Name.Equals(group.Name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException(
                $"Expense group with name '{group.Name}' already exists for this user.");
        }

        await groupRepository.CreateGroupAsync(group);

        return group.ToDetailsDto();
    }

    public async Task<ExpenseGroupDetailsDto?> GetGroupByIdAsync(int id)
    {
        var userId = userServiceContext.GetCurrentUserId();

        var group = await groupRepository.GetGroupByIdAsync(id, userId);

        if (group == null)
        {
            return null;
        }

        return group.ToDetailsDto();
    }

    public async Task<List<ExpenseGroupListDto>> GetAllGroupsForUserAsync()
    {
        var userId = userServiceContext.GetCurrentUserId();

        var groups = await groupRepository.GetAllGroupsAsync(userId);

        return groups.Select(ExpenseGroupMapper.ToListDto).ToList();
    }

    public async Task<ExpenseGroupDetailsDto> UpdateGroupAsync(int id, ExpenseGroupUpdateDto dto)
    {
        var userId = userServiceContext.GetCurrentUserId();

        var existingGroup = await groupRepository.GetGroupByIdAsync(id, userId);

        if (existingGroup == null)
        {
            throw new KeyNotFoundException("Expense Group with this ID not found or unauthorized.");
        }

        existingGroup.Name = dto.Name;
        existingGroup.MonthlyLimit = dto.MonthlyLimit;

        return existingGroup.ToDetailsDto();
    }

    public async Task<bool> DeleteGroupAsync(int id)
    {
        var userId = userServiceContext.GetCurrentUserId();

        var existingGroup = await groupRepository.GetGroupByIdAsync(id, userId);

        var expensesCount = await expenseRepository.CountExpensesInGroupAsync(id, userId);

        if (existingGroup == null)
        {
            return false;
        }

        //we will work on enabling cascade deletion
        if (expensesCount > 0)
        {
            throw new InvalidOperationException("Cannot delete group - there are some expenses left in it.");
        }

        await groupRepository.DeleteGroupAsync(existingGroup);

        return true;
    }
}