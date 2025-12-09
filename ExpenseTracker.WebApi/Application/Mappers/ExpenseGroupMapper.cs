using ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.Mappers;

public static class ExpenseGroupMapper
{
    public static ExpenseGroup ToEntity(this ExpenseGroupCreateDto dto, string userId) =>
        new ExpenseGroup
        {
            UserId = userId,
            Name = dto.Name,
            MonthlyLimit = dto.MonthlyLimit
        };

    public static void MapToEntity(this ExpenseGroupUpdateDto dto, ExpenseGroup entity)
    {
        entity.Name = dto.Name;
        entity.MonthlyLimit = dto.MonthlyLimit;
    }

    public static ExpenseGroupDetailsDto ToDetailsDto(this ExpenseGroup group) =>
        new ExpenseGroupDetailsDto(
            group.Id,
            group.Name,
            group.MonthlyLimit,
            group.Expenses.Count
        );

    public static ExpenseGroupListDto ToListDto(this ExpenseGroup group) =>
        new ExpenseGroupListDto(
            group.Id,
            group.Name,
            group.MonthlyLimit,
            group.Expenses.Count
        );
}
