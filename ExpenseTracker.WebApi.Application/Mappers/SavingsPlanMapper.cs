using ExpenseTracker.WebApi.Application.DTOs.Savings;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.Mappers;

public static class SavingsPlanMapper
{
    public static SavingsPlan ToEntity(
        this SavingsPlanCreateDto dto,
        Guid userId,
        IEnumerable<SavingsPlanContribution> contributions)
    {
        return new SavingsPlan
        {
            TargetAmount = dto.TargetAmount,
            TargetDate = dto.TargetDate,
            UserId = userId,
            Contributions = contributions.ToList()
        };
    }

    public static SavingsPlanDto ToDto(this SavingsPlan plan)
    {
        var isCompleted =
            plan.Contributions.Any() &&
            plan.Contributions.All(c => c.IsCompleted);

        return new SavingsPlanDto(
            plan.Id,
            plan.TargetAmount,
            plan.TargetDate,
            isCompleted,
            plan.Contributions.Select(c =>
                new SavingsPlanContributionDto(
                    new DateTime(c.Year, c.Month, 1),
                    c.PlannedAmount,
                    c.ActualAmount
                )).ToList()
        );
    }
}
