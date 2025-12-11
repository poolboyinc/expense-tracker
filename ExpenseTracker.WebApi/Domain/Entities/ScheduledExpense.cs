using ExpenseTracker.WebApi.Domain.Enums;

namespace ExpenseTracker.WebApi.Domain.Entities;

#nullable disable



public class ScheduledExpense
{
    public int Id { get; set; }

    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;

    public DateTime NextRunAt { get; set; }
    
    public DateTime? EndAt { get; set; }

    public RecurrenceFrequency Frequency { get; set; } = RecurrenceFrequency.Monthly;
    
    public int? DayOfWeek { get; set; }
    
    public int? DayOfMonth { get; set; }

    public bool IsActive { get; set; } = true;
    
    public int ExpenseGroupId { get; set; }
    public ExpenseGroup ExpenseGroup { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

#nullable restore
