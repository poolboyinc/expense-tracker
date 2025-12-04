using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ApplicationDbContext _context;
    
    public ExpenseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<Expense> GetByIdAsync(int id)
    {
        return _context.Expenses
            .Include( e => e.ExpenseGroup)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Expense expense)
    {
        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expense expense)
    {
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
    }

    public Task<ExpenseGroup?> GetGroupByIdAsync(int groupId)
    {
        return _context.ExpenseGroups.FindAsync(groupId).AsTask();
    }

    public async Task<ICollection<Expense>> GetExpensesAsync(
        string userId, 
        int? groupId, 
        string? searchTerm, 
        int pageNumber, 
        int pageSize)
    {
        var query = _context.Expenses
            .Include(e => e.ExpenseGroup) 
            .Where(e => e.UserId == userId)
            .AsQueryable(); 
        
        if (groupId.HasValue)
        {
            query = query.Where(e => e.ExpenseGroupId == groupId.Value);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(e => e.Description.ToLower().Contains(term));
        }
        
        query = query.OrderByDescending(e => e.TransactionDate);
        
        var skipAmount = (pageNumber - 1) * pageSize;

        return await query
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync(); 
    }
    
    
}