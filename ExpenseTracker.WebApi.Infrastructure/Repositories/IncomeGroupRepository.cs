using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class IncomeGroupRepository(ApplicationDbContext context) : IIncomeGroupRepository
{
    public async Task<IncomeGroup> CreateAsync(IncomeGroup group)
    {
        await context.IncomeGroup.AddAsync(group);
        await context.SaveChangesAsync();
        return group;
    }

    public async Task<IncomeGroup?> GetByIdAsync(int id)
    {
        return await context.IncomeGroup
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<List<IncomeGroup>> GetAllByUserIdAsync(Guid userId)
    {
        return await context.IncomeGroup
            .Where(g => g.UserId == userId)
            .OrderBy(g => g.Name)
            .ToListAsync();
    }

    public async Task<IncomeGroup> UpdateAsync(IncomeGroup group)
    {
        context.IncomeGroup.Update(group);
        await context.SaveChangesAsync();
        return group;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var group = await context.IncomeGroup.FindAsync(id);
        if (group == null) return false;

        context.IncomeGroup.Remove(group);
        await context.SaveChangesAsync();
        return true;
    }
}