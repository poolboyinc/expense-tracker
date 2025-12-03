using ExpenseTracker.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseGroup> ExpenseGroups { get; set; }
    
}