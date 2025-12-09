using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class ExpenseRepository(ApplicationDbContext context) : IExpenseRepository
{
    public async Task<List<Expense>> GetAllExpenses(string userId)
    {
        return await context.Expenses
            .Where(e => e.UserId == userId)
            .Include(e => e.ExpenseGroup) 
            .ToListAsync();
    }
    
    public async Task<Expense?> GetByIdAsync(int id, string userId)
    {
        return await context.Expenses
            .Include(e => e.ExpenseGroup)
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
    }
    
    public async Task AddAsync(Expense expense)
    {
        await context.Expenses.AddAsync(expense);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Expense expense)
    {
        context.Expenses.Update(expense);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Expense expense)
    {
        context.Expenses.Remove(expense);
        await context.SaveChangesAsync();
    }


    public async Task<ExpenseGroup?> GetGroupByIdAsync(int groupId)
    {
        return await context.ExpenseGroups.FirstOrDefaultAsync(g => g.Id == groupId);
    }


    public async Task<List<Expense>> GetExpensesAsync(
        string userId,
        int? groupId,
        string? searchTerm,
        int pageNumber,
        int pageSize
    )
    {

        var query = context.Expenses
            .Where(e => e.UserId == userId)
            .Include(e => e.ExpenseGroup)
            .AsQueryable();

        if (groupId.HasValue)
        {
            query = query.Where(e => e.ExpenseGroupId == groupId.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(e => e.Description.ToLower().Contains(searchTerm.ToLower()));
        }

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountExpensesInGroupAsync(int groupId, string userId)
    {
        return await context.Expenses.CountAsync(e => e.ExpenseGroupId == groupId && e.UserId == userId);
    }
}