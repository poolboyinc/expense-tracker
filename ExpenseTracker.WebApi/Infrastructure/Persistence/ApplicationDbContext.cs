using ExpenseTracker.WebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Expense> Expense { get; set; }
    public DbSet<ExpenseGroup> ExpenseGroup { get; set; }

    public DbSet<Income> Income { get; set; }
    public DbSet<IncomeGroup> IncomeGroup { get; set; }

    public DbSet<User> User { get; set; }
    
    public DbSet<ScheduledExpense> ScheduledExpense { get; set; }


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
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Income>()
            .HasOne<User>()
            .WithMany(u => u.Incomes)
            .HasForeignKey(i => i.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ExpenseGroup>()
            .HasOne<User>()
            .WithMany(u => u.ExpenseGroups)
            .HasForeignKey(g => g.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<IncomeGroup>()
            .HasOne<User>()
            .WithMany(u => u.IncomeGroups)
            .HasForeignKey(g => g.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ScheduledExpense>()
            .HasOne(se => se.User)
            .WithMany(u => u.ScheduledExpenses)
            .HasForeignKey(se => se.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<ScheduledExpense>()
            .HasOne(se => se.ExpenseGroup)
            .WithMany(g => g.ScheduledExpenses)
            .HasForeignKey(se => se.ExpenseGroupId)
            .IsRequired();
    }
}