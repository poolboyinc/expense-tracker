using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class ScheduledExpenseRepository( ApplicationDbContext context) : IScheduledExpenseRepository
{

    public async Task<ScheduledExpense> CreateAsync(ScheduledExpense item)
    {
        await context.AddAsync(item);
        await context.SaveChangesAsync();
        return item;
    }

    public async Task<ScheduledExpense?> GetByIdAsync(int id)
    {
        return await context.Set<ScheduledExpense>()
            .Include(s => s.ExpenseGroup)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<ScheduledExpense>> GetAllForUserAsync(Guid userId)
    {
        return await context.Set<ScheduledExpense>()
            .Where(s => s.UserId == userId)
            .Include(s => s.ExpenseGroup)
            .ToListAsync();
    }

    public async Task UpdateAsync(ScheduledExpense item)
    {
        context.Update(item);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ScheduledExpense item)
    {
        context.Remove(item);
        await context.SaveChangesAsync();
    }

    public async Task<List<ScheduledExpense>> GetDueScheduledExpensesAsync(DateTime utcNow)
    {
       return await context.Set<ScheduledExpense>()
            .Include(s => s.ExpenseGroup)
            .Where(s => s.IsActive && s.NextRunAt <= utcNow && (s.EndAt == null || s.EndAt >= utcNow))
            .ToListAsync();
    }
}
