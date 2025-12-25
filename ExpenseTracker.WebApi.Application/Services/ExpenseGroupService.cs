using ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class ExpenseGroupService(
    IExpenseGroupRepository groupRepository,
    IUserServiceContext userServiceContext)
    : IExpenseGroupService
{
    public async Task<ExpenseGroupDetailsDto> CreateGroupAsync(ExpenseGroupCreateDto dto)
    {
        var userId = userServiceContext.GetCurrentUserId();
        var group = dto.ToEntity(userId);

        var groupExists = await groupRepository.GetGroupByIdAsync(group.Id, userId);

        if (groupExists != null)
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

        if (existingGroup == null)
        {
            return false;
        }

        await groupRepository.DeleteGroupAsync(existingGroup);

        return true;
    }

    public async Task<decimal> GetTotalExpensesForGroupThisMonthAsync(int groupId)
    {
        var userId =  userServiceContext.GetCurrentUserId();
        return await  groupRepository.GetTotalExpensesForGroupThisMonthAsync(groupId, userId);
    }

    public async Task<decimal> GetTotalExpensesForGroupInRangeAsync(int groupId, DateTime from, DateTime to)
    {
        var userId = userServiceContext.GetCurrentUserId();
        return await groupRepository.GetTotalExpensesForGroupInRangeAsync(groupId, userId, from, to);
    }
    
    public async Task<BudgetStatusDto> GetBudgetStatusAsync(int groupId)
    {
        var userId = userServiceContext.GetCurrentUserId();
        var group = await groupRepository.GetGroupByIdAsync(groupId, userId);

        if (group == null)
        {
            throw new KeyNotFoundException();
        }

        var spent = await groupRepository
            .GetTotalExpensesForGroupThisMonthAsync(groupId, userId);

        var limit = group.MonthlyLimit;

        return new BudgetStatusDto(
            limit,
            spent,
            limit.HasValue ? limit.Value - spent : 0,
            limit.HasValue && spent > limit.Value
        );
    }

}