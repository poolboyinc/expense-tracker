using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class SavingsAvailabilityService(
    IIncomeRepository incomeRepository,
    IExpenseRepository expenseRepository,
    ISavingsPlanRepository savingsPlanRepository)
    : ISavingsAvailabilityService
{
    public async Task<bool> CanSpendAsync(Guid userId, decimal amount)
    {
        var now = DateTime.UtcNow;

        var income = await incomeRepository.GetTotalIncomeForMonthAsync(
            userId, now.Year, now.Month);

        var expenses = await expenseRepository.GetTotalForMonthAsync(
            userId, now.Year, now.Month);

        var requiredSavings = await savingsPlanRepository.GetRequiredSavingsSumAsync(userId, now.Year, now.Month);

        var available = income - expenses - requiredSavings;

        return available >= amount;
    }
}
