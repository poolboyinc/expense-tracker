namespace ExpenseTracker.WebApi.Domain.Entities;

public class Expense
{
    public int Id  { get; set; }
    
    public decimal Amount { get; set; }
    
    public string Description { get; set; }
    
    public DateTime TransactionDate { get; set; }
    
    public int ExpenseGroupId { get; set; }

    public ExpenseGroup ExpenseGroup { get; set; } = default!;
    
    public bool IsScheduled { get; set; }
    
    public string UserId { get; set; } = string.Empty; 
}