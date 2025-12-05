using ExpenseTracker.WebApi.Application.ServiceContracts;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class IncomeService : IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IExpenseGroupRepository _groupRepository; 

    public IncomeService(IIncomeRepository incomeRepository, 
                         IUserRepository userRepository,
                         IExpenseGroupRepository groupRepository) 
    {
        _incomeRepository = incomeRepository;
        _userRepository = userRepository;
        _groupRepository = groupRepository;
    }
    private async Task ValidateIncomeDataAsync(Income income)
    {
        var userExists = await _userRepository.UserExistsAsync(income.UserId);
        if (!userExists)
        {
            throw new KeyNotFoundException($"User with this ID not found.");
        }
        
        var groupExists = await _groupRepository.GroupExistsAsync(income.IncomeGroupId); 
        if (!groupExists)
        {
            throw new KeyNotFoundException($"Income Group with this ID not found.");
        }
    }
    
    public async Task<Income> CreateIncomeAsync(Income income, string userId)
    {
        income.UserId = userId; 
        
        await ValidateIncomeDataAsync(income);

        return await _incomeRepository.CreateIncomeAsync(income);
    }
    
    public async Task<Income?> GetIncomeByIdAsync(int id, string userId)
    {
        var income = await _incomeRepository.GetIncomeByIdAsync(id);
        
        if (income != null && income.UserId != userId)
        {
            return null; 
        }

        return income;
    }
    
    public Task<List<Income>> GetAllIncomesForUserAsync(string userId)
    {
        return _incomeRepository.GetAllIncomesByUserIdAsync(userId);
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
        
        return await _incomeRepository.UpdateIncomeAsync(income);
    }
    
    public async Task<bool> DeleteIncomeAsync(int id, string userId)
    {
        var incomeToDelete = await GetIncomeByIdAsync(id, userId);

        if (incomeToDelete == null)
        {
            return false;
        }

        return await _incomeRepository.DeleteIncomeAsync(id);
    }
}