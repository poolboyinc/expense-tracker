using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class ScheduledIncomeRepository(ApplicationDbContext context) : IScheduledIncomeRepository
{
    public async Task<List<ScheduledIncome>> GetDueAsync(DateTime now)
    {
        return await context.ScheduledIncome
            .Where(s => s.IsActive && s.NextRunAt <= now)
            .ToListAsync();
    }


    public async Task CreateAsync(ScheduledIncome income)
    {
        await context.AddAsync(income);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ScheduledIncome income)
    {
        context.Update(income);
        await context.SaveChangesAsync();
    }
}