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
        await context.Incomes.AddAsync(income);
        await context.SaveChangesAsync();
        return income;
    }

    public async Task<Income?> GetIncomeByIdAsync(int id)
    {

        return await context.Incomes
            .Include(i => i.IncomeGroup) 
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Income>> GetAllIncomesByUserIdAsync(string userId)
    {
        return await context.Incomes
            .Where(i => i.UserId == userId)
            .Include(i => i.IncomeGroup)
            .OrderByDescending(i => i.Date) 
            .ToListAsync();
    }

    public async Task<Income> UpdateIncomeAsync(Income income)
    {
        context.Incomes.Update(income);
        await context.SaveChangesAsync(); 
        return income;
    }
    
    public async Task<bool> DeleteIncomeAsync(int id)
    {
        var incomeToDelete = await context.Incomes.FindAsync(id);

        if (incomeToDelete == null)
        {
            return false;
        }

        context.Incomes.Remove(incomeToDelete);
        await context.SaveChangesAsync();
        
        return true; 
    }
}
