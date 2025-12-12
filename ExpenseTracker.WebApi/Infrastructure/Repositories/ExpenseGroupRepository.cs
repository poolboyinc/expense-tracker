using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class ExpenseGroupRepository(ApplicationDbContext context) : IExpenseGroupRepository
{
    public async Task<List<ExpenseGroup>> GetAllGroupsAsync(Guid userId)
    {
        return await context.ExpenseGroup.Where(g => g.UserId == userId).ToListAsync();
    }

    public async Task<ExpenseGroup?> GetGroupByIdAsync(int id, Guid userId)
    {
        return await context.ExpenseGroup.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
    }

    public async Task<ExpenseGroup> CreateGroupAsync(ExpenseGroup group)
    {
        await context.ExpenseGroup.AddAsync(group);
        await context.SaveChangesAsync();
        return group;
    }

    public async Task<ExpenseGroup> UpdateGroupAsync(ExpenseGroup group)
    {
        context.ExpenseGroup.Update(group);
        await context.SaveChangesAsync();
        return group;
    }

    public async Task DeleteGroupAsync(ExpenseGroup group)
    {
        context.ExpenseGroup.Remove(group);
        await context.SaveChangesAsync();
    }

    public async Task<decimal> GetTotalExpensesForGroupThisMonthAsync(int groupId, Guid userId)
    {
        var now = DateTime.UtcNow;

        return await context.Expense
            .Where(e => e.UserId == userId &&
                        e.ExpenseGroupId == groupId &&
                        e.TransactionDate.Year == now.Year &&
                        e.TransactionDate.Month == now.Month)
            .SumAsync(e => e.Amount);
    }
    
    public async Task<decimal> GetTotalExpensesForGroupInRangeAsync(
        int groupId,
        Guid userId,
        DateTime from,
        DateTime to)
    {
        return await context.Expense
            .Where(e => e.UserId == userId &&
                        e.ExpenseGroupId == groupId &&
                        e.TransactionDate >= from &&
                        e.TransactionDate <= to)
            .SumAsync(e => e.Amount);
    }


}