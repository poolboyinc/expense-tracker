namespace ExpenseTracker.WebApi.Domain.Entities;

public class SavingsPlanContribution
{
    public int Id { get; set; }
    
    public int SavingsPlanId { get; set; }
    public SavingsPlan SavingsPlan { get; set; } = null!;
    
    public int Year { get; set; }
    public int Month { get; set; }
    
    public decimal PlannedAmount { get; set; }
    public decimal ActualAmount { get; set; }
    
    public bool IsCompleted { get; set; }
}