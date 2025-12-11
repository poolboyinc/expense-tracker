namespace ExpenseTracker.WebApi.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsPremium { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<ExpenseGroup> ExpenseGroups { get; set; } = new List<ExpenseGroup>();
    public ICollection<IncomeGroup> IncomeGroups { get; set; } = new List<IncomeGroup>();
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
    
    public ICollection<ScheduledExpense> ScheduledExpenses { get; set; } = new List<ScheduledExpense>();

}