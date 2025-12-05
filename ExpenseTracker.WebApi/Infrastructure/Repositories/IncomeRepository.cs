using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly ApplicationDbContext _context;

    public IncomeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Income> CreateIncomeAsync(Income income)
    {
        await _context.Incomes.AddAsync(income);
        await _context.SaveChangesAsync();
        return income;
    }

    public async Task<Income?> GetIncomeByIdAsync(int id)
    {

        return await _context.Incomes
            .Include(i => i.IncomeGroup) 
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Income>> GetAllIncomesByUserIdAsync(string userId)
    {
        return await _context.Incomes
            .Where(i => i.UserId == userId)
            .Include(i => i.IncomeGroup)
            .OrderByDescending(i => i.Date) 
            .ToListAsync();
    }

    public async Task<Income> UpdateIncomeAsync(Income income)
    {
        _context.Incomes.Update(income);
        await _context.SaveChangesAsync(); 
        return income;
    }
    
    public async Task<bool> DeleteIncomeAsync(int id)
    {
        var incomeToDelete = await _context.Incomes.FindAsync(id);

        if (incomeToDelete == null)
        {
            return false;
        }

        _context.Incomes.Remove(incomeToDelete);
        await _context.SaveChangesAsync();
        
        return true; 
    }
}
