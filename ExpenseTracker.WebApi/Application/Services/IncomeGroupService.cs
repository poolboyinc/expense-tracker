using ExpenseTracker.WebApi.Application.DTOs.IncomeGroup;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class IncomeGroupService(
    IIncomeGroupRepository repository, 
    IUserServiceContext userServiceContext) : IIncomeGroupService
{
    public async Task<IncomeGroupDto> CreateAsync(IncomeGroupCreateDto dto)
    {
        var group = new IncomeGroup
        {
            Name = dto.Name,
            UserId = userServiceContext.GetCurrentUserId()
        };

        await repository.CreateAsync(group);
        return new IncomeGroupDto(group.Id, group.Name);
    }

    public async Task<IncomeGroupDto?> GetByIdAsync(int id)
    {
        var group = await repository.GetByIdAsync(id);
        return group == null ? null : new IncomeGroupDto(group.Id, group.Name);
    }

    public async Task<List<IncomeGroupDto>> GetAllForUserAsync()
    {
        var userId = userServiceContext.GetCurrentUserId();
        var groups = await repository.GetAllByUserIdAsync(userId);
        return groups.Select(g => new IncomeGroupDto(g.Id, g.Name)).ToList();
    }

    public async Task UpdateAsync(int id, IncomeGroupUpdateDto dto)
    {
        var existing = await repository.GetByIdAsync(id);
        var currentUserId = userServiceContext.GetCurrentUserId();

        if (existing == null || existing.UserId != currentUserId)
        {
            throw new KeyNotFoundException("Income Group not found or access denied.");
        }

        existing.Name = dto.Name;
        await repository.UpdateAsync(existing);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await repository.GetByIdAsync(id);
        var currentUserId = userServiceContext.GetCurrentUserId();

        if (existing == null || existing.UserId != currentUserId)
        {
            return false;
        }

        return await repository.DeleteAsync(id);
    }
}