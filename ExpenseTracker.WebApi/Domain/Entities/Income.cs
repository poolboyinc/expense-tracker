namespace ExpenseTracker.WebApi.Domain.Entities;

public class Income
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public bool IsScheduled { get; set; } = false;

    public int IncomeGroupId { get; set; }
    public IncomeGroup IncomeGroup { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
}