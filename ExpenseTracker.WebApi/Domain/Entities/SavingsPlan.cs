namespace ExpenseTracker.WebApi.Domain.Entities;

public class SavingsPlan
{
    public int Id { get; set; }

    // Ownership
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    // Goal
    public decimal TargetAmount { get; set; }
    public DateTime TargetDate { get; set; }

    // Lifecycle
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<SavingsPlanContribution> Contributions { get; set; }
        = new List<SavingsPlanContribution>();
}