using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class IncomeRepository(ApplicationDbContext context) : IIncomeRepository
{
    public async Task<Income> CreateIncomeAsync(Income income)
    {
        await context.Income.AddAsync(income);
        await context.SaveChangesAsync();
        return income;
    }

    public async Task<Income?> GetIncomeByIdAsync(int id)
    {

        return await context.Income
            .Include(i => i.IncomeGroup) 
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Income>> GetAllIncomesByUserIdAsync(string userId)
    {
        return await context.Income
            .Where(i => i.UserId == userId)
            .Include(i => i.IncomeGroup)
            .OrderByDescending(i => i.Date) 
            .ToListAsync();
    }

    public async Task<Income> UpdateIncomeAsync(Income income)
    {
        context.Income.Update(income);
        await context.SaveChangesAsync(); 
        return income;
    }
    
    public async Task<bool> DeleteIncomeAsync(int id)
    {
        var incomeToDelete = await context.Income.FindAsync(id);

        if (incomeToDelete == null)
        {
            return false;
        }

        context.Income.Remove(incomeToDelete);
        await context.SaveChangesAsync();
        
        return true; 
    }
}
