using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class ExpenseGroupRepository : IExpenseGroupRepository
{
    private readonly ApplicationDbContext _context;

    public ExpenseGroupRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<ExpenseGroup> CreateGroupAsync(ExpenseGroup group)
    {
        await _context.ExpenseGroups.AddAsync(group);
        await _context.SaveChangesAsync();
        return group;
    }
    
    public async Task<ExpenseGroup?> GetGroupByIdAsync(int id)
    {
        return await _context.ExpenseGroups
            .FirstOrDefaultAsync(g => g.Id == id);
    }
    
    public async Task<List<ExpenseGroup>> GetGroupsByUserIdAsync(string userId)
    {
        return await _context.ExpenseGroups
            .Where(g => g.UserId == userId)
            .OrderBy(g => g.Name)
            .ToListAsync();
    }
    
    public async Task<ExpenseGroup> UpdateGroupAsync(ExpenseGroup group)
    {
        _context.ExpenseGroups.Update(group);
        await _context.SaveChangesAsync(); 
        return group;
    }
    
    public async Task<bool> DeleteGroupAsync(int id)
    {
        var groupToDelete = await _context.ExpenseGroups.FindAsync(id);

        if (groupToDelete == null)
        {
            return false;
        }
        
        //configure cascade deletion in ef core

        _context.ExpenseGroups.Remove(groupToDelete);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<bool> GroupExistsAsync(int id)
    {
        return await _context.ExpenseGroups.AnyAsync(g => g.Id == id);
    }
    
}