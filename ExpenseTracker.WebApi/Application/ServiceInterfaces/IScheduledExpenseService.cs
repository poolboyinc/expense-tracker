using ExpenseTracker.WebApi.Application.DTOs.ScheduledExpense;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IScheduledExpenseService
{
    Task<ScheduledExpenseDto> CreateAsync(ScheduledExpenseCreateDto dto);

    Task<List<ScheduledExpenseDto>> GetAllForCurrentUserAsync();

    Task<ScheduledExpenseDto?> GetByIdAsync(int id);

    Task UpdateAsync(ScheduledExpenseUpdateDto dto);

    Task DeleteAsync(int id);

    Task<int> ProcessDueScheduledExpensesAsync(DateTime utcNow);
    
}