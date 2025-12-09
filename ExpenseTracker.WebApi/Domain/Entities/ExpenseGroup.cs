namespace ExpenseTracker.WebApi.Domain.Entities;

public class ExpenseGroup
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public decimal? MonthlyLimit { get; set; }

    public required string UserId { get; set; } = string.Empty;
    
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}