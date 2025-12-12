using ExpenseTracker.WebApi.Application.DTOs.Income;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class IncomeService(
    IIncomeRepository incomeRepository,
    IUserServiceContext userServiceContext)
    : IIncomeService
{
    public async Task<IncomeDto> CreateIncomeAsync(IncomeCreateDto dto)
    {
        var income = IncomeMapper.FromCreateDto(dto);

        income.UserId = userServiceContext.GetCurrentUserId();

        await ValidateIncomeDataAsync(income);

        await incomeRepository.CreateIncomeAsync(income);

        return IncomeMapper.ToDto(income);
    }

    public async Task<IncomeDto?> GetIncomeByIdAsync(int id)
    {
        var income = await incomeRepository.GetIncomeByIdAsync(id);

        if (income == null)
        {
            return null;
        }

        return IncomeMapper.ToDto(income);
    }

    public async Task<List<IncomeDto>> GetAllIncomesForUserAsync()
    {
        var userId = userServiceContext.GetCurrentUserId();
        var incomes = await incomeRepository.GetAllIncomesByUserIdAsync(userId);
        return incomes.Select(IncomeMapper.ToDto).ToList();
    }

    public async Task UpdateIncomeAsync(int id, IncomeUpdateDto dto)
    {
        var existingIncome = await incomeRepository.GetIncomeByIdAsync(id);

        if (existingIncome == null)
        {
            throw new KeyNotFoundException("Income with ID  not found or you do not have permission.");
        }

        await ValidateIncomeDataAsync(existingIncome);

        await incomeRepository.UpdateIncomeAsync(existingIncome);
    }

    public async Task<bool> DeleteIncomeAsync(int id)
    {
        var incomeToDelete = await incomeRepository.GetIncomeByIdAsync(id);

        if (incomeToDelete == null)
        {
            return false;
        }

        return await incomeRepository.DeleteIncomeAsync(id);
    }

    private async Task ValidateIncomeDataAsync(Income income)
    {
        var userId = userServiceContext.GetCurrentUserId();

        var incomeGroup = await incomeRepository.GetIncomeByIdAsync(income.Id);

        if (incomeGroup == null)
        {
            throw new KeyNotFoundException("Income with ID  not found.");
        }
    }
}