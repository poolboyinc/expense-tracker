using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class SavingsPlanRepository(ApplicationDbContext context) : ISavingsPlanRepository
{
    public async Task<SavingsPlan?> GetByIdAsync(int id, Guid userId)
    {
        return await context.SavingsPlan
            .Include(p => p.Contributions)
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
    }

    public async Task<List<SavingsPlan>> GetAllAsync(Guid userId)
    {
        return await context.SavingsPlan
            .Where(p => p.UserId == userId)
            .Include(p => p.Contributions)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<SavingsPlan> CreateAsync(SavingsPlan plan)
    {
        await context.SavingsPlan.AddAsync(plan);
        await context.SaveChangesAsync();
        return plan;
    }

    public async Task UpdateAsync(SavingsPlan plan)
    {
        context.SavingsPlan.Update(plan);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<SavingsPlan>> GetAllActiveAsync()
    {
        return await context.SavingsPlan
            .Include(p => p.Contributions).
            Include(p => p.User)
            .Where(p => p.IsActive)
            .ToListAsync();
    }


    public async Task DeleteAsync(SavingsPlan plan)
    {
        context.SavingsPlan.Remove(plan);
        await context.SaveChangesAsync();
    }
}