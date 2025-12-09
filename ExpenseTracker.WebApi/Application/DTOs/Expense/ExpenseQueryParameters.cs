namespace ExpenseTracker.WebApi.Application.DTOs.Expense;

public class ExpenseQueryParameters
{
    public int? GroupId { get; set; }
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}