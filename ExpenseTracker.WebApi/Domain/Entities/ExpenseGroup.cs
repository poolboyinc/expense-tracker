namespace ExpenseTracker.WebApi.Domain.Entities;

public class ExpenseGroup
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public decimal? MonthlyLimit { get; set; }
    
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}