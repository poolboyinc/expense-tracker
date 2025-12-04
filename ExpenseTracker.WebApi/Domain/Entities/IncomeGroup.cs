namespace ExpenseTracker.WebApi.Domain.Entities;

public class IncomeGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
}