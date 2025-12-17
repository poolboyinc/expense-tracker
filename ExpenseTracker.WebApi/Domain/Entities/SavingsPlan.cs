namespace ExpenseTracker.WebApi.Domain.Entities;

public class SavingsPlan
{
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public decimal TargetAmount { get; set; }
    public DateTime TargetDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    
    public ICollection<SavingsPlanContribution> Contributions { get; set; }
        = new List<SavingsPlanContribution>();
    
    public bool GoalReachedNotified { get; set; }

}