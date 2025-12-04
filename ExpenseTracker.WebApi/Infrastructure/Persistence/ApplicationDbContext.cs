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
    
    public DbSet<Income> Incomes { get; set; }
    public DbSet<IncomeGroup> IncomeGroups { get; set; }
    
    public DbSet<User> Users { get; set; } 
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.ExpenseGroup)         
            .WithMany(g => g.Expenses)             
            .HasForeignKey(e => e.ExpenseGroupId)  
            .IsRequired();
        
        modelBuilder.Entity<Income>()
            .HasOne(i => i.IncomeGroup)        
            .WithMany(g => g.Incomes)          
            .HasForeignKey(i => i.IncomeGroupId)   
            .IsRequired();
        
        modelBuilder.Entity<Expense>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .IsRequired();
        
        modelBuilder.Entity<Income>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(i => i.UserId)
            .IsRequired();
        
    }
}