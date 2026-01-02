using ExpenseTracker.WebApi.Application.DTOs.IncomeGroup;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.Mappers;

public static class IncomeGroupMapper
{
    public static IncomeGroupDto ToDto(this IncomeGroup entity)
    {
        return new IncomeGroupDto(entity.Id, entity.Name);
    }

    public static IncomeGroup ToEntity(this IncomeGroupCreateDto dto, Guid userId)
    {
        return new IncomeGroup
        {
            Name = dto.Name,
            UserId = userId
        };
    }

    public static List<IncomeGroupDto> ToDtoList(this IEnumerable<IncomeGroup> entities)
    {
        return entities.Select(ToDto).ToList();
    }
}