using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class IncomeService(
    IIncomeRepository incomeRepository,
    IUserServiceContext userServiceContext,
    IExpenseGroupRepository groupRepository)
    : IIncomeService
{
    private async Task ValidateIncomeDataAsync(Income income)
    {
        var userId = userServiceContext.GetCurrentUserId();
        
        if (userId == null)
        {
            throw new KeyNotFoundException($"User with this ID not found.");
        }
        
        var expenseGroup = await groupRepository.GetGroupByIdAsync(income.IncomeGroupId, userId); 
        if (expenseGroup == null)
        {
            throw new KeyNotFoundException($"Income Group with this ID not found.");
        }
    }
    
    public async Task<Income> CreateIncomeAsync(Income income, string userId)
    {
        income.UserId = userId; 
        
        await ValidateIncomeDataAsync(income);

        return await incomeRepository.CreateIncomeAsync(income);
    }
    
    public async Task<Income?> GetIncomeByIdAsync(int id, string userId)
    {
        var income = await incomeRepository.GetIncomeByIdAsync(id);
        
        if (income != null && income.UserId != userId)
        {
            return null; 
        }

        return income;
    }
    
    public Task<List<Income>> GetAllIncomesForUserAsync(string userId)
    {
        return incomeRepository.GetAllIncomesByUserIdAsync(userId);
    }
    
    public async Task<Income> UpdateIncomeAsync(Income income, string userId)
    {
        var existingIncome = await GetIncomeByIdAsync(income.Id, userId);
        if (existingIncome == null)
        {
            throw new KeyNotFoundException($"Income with ID  not found or you do not have permission.");
        }
        
        income.UserId = userId; 
        
        await ValidateIncomeDataAsync(income);
        
        return await incomeRepository.UpdateIncomeAsync(income);
    }
    
    public async Task<bool> DeleteIncomeAsync(int id, string userId)
    {
        var incomeToDelete = await GetIncomeByIdAsync(id, userId);

        if (incomeToDelete == null)
        {
            return false;
        }

        return await incomeRepository.DeleteIncomeAsync(id);
    }
}