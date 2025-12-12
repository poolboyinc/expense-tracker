namespace ExpenseTracker.WebApi.Domain.Entities;

public class ExpenseGroup
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal? MonthlyLimit { get; set; }

    public required Guid UserId { get; set; }

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}