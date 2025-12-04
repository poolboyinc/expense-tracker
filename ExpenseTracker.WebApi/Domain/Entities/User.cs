namespace ExpenseTracker.WebApi.Domain.Entities;

public class User
{
    public string Id  { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public bool IsPremium { get; set; }
}