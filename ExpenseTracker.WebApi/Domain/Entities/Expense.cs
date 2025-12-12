namespace ExpenseTracker.WebApi.Domain.Entities;
#nullable disable
public class Expense
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public required string Description { get; set; }

    public DateTime TransactionDate { get; set; }

    public int ExpenseGroupId { get; set; }

    public ExpenseGroup ExpenseGroup { get; set; }

    public bool IsScheduled { get; set; }

    public Guid UserId { get; set; }
}