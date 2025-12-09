using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Repositories;

namespace ExpenseTracker.WebApi.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IUserServiceContext _userServiceContext;

    public ExpenseService(IExpenseRepository expenseRepository,  IUserServiceContext userServiceContext)
    {
        _expenseRepository = expenseRepository;
        _userServiceContext = userServiceContext;
    }

    public async Task<List<Expense>> GetAllExpensesAsync(string userId)
    {
        var expenses = await _expenseRepository.GetAllExpenses(userId);
        
        return expenses;
    }

    public async Task<Expense?> GetExpenseByIdAsync(int id, string userId)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, userId);
        
        return expense;
    }

    public async Task<Expense> CreateExpenseAsync(Expense expense, string userId)
    {
        var group = await _expenseRepository.GetGroupByIdAsync(expense.ExpenseGroupId);

        if (group == null)
        {
            throw new InvalidOperationException();
        }
        
        expense.UserId = userId;
        
        await _expenseRepository.AddAsync(expense);
        
        return expense;
    }

    public async Task<ICollection<Expense>> GetFilteredExpensesAsync(string userId, int? groupId, string? searchTerm, int pageNumber, int pageSize)
    {
        return await _expenseRepository.GetExpensesAsync(userId, groupId, searchTerm, pageNumber, pageSize);
    }
    
    public async Task UpdateExpenseAsync(Expense expense, string userId)
    {
        var existingExpense = await _expenseRepository.GetByIdAsync(expense.Id, userId);

        if (existingExpense == null || existingExpense.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        if (existingExpense.ExpenseGroupId != expense.ExpenseGroupId)
        {
            var group = await _expenseRepository.GetGroupByIdAsync(expense.ExpenseGroupId);
            
            if (group == null)
            {
                throw new InvalidOperationException();
            }
            
        }
        
        existingExpense.Amount = expense.Amount;
        existingExpense.Description = expense.Description;
        existingExpense.ExpenseGroupId = expense.ExpenseGroupId;
        existingExpense.TransactionDate = expense.TransactionDate;
        
        await _expenseRepository.UpdateAsync(existingExpense);
    }


    public async Task DeleteExpenseAsync(int id, string userId)
    {
        var expense = await _expenseRepository.GetByIdAsync(id, userId);

        if (expense == null)
        {
            throw new InvalidOperationException();
        }

        await _expenseRepository.DeleteAsync(expense);
    }
    
}