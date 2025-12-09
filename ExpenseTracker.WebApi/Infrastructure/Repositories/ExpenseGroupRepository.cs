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
    
    public async Task<List<ExpenseGroup>> GetAllGroupsAsync(string userId)
    {
        return await _context.ExpenseGroups.Where(g => g.UserId == userId).ToListAsync();
    }

    public async Task<ExpenseGroup?> GetGroupByIdAsync(int id, string userId)
    {
        return await  _context.ExpenseGroups.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
    }

    
    public async Task<ExpenseGroup> CreateGroupAsync(ExpenseGroup group)
    {
        await _context.ExpenseGroups.AddAsync(group);
        await _context.SaveChangesAsync();
        return group;
    }
    
    public async Task<ExpenseGroup> UpdateGroupAsync(ExpenseGroup group)
    {
        _context.ExpenseGroups.Update(group);
        await _context.SaveChangesAsync(); 
        return group;
    }

    public async Task DeleteGroupAsync(ExpenseGroup group)
    {
        _context.ExpenseGroups.Remove(group);
        await _context.SaveChangesAsync();
    }
    
    
}