using ExpenseTracker.WebApi.Application.DTOs.Expense;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IExpenseService
{
    Task<ExpenseDetailsDto?> GetExpenseByIdAsync(int id);

    Task<ExpenseDetailsDto> CreateExpenseAsync(ExpenseCreateDto dto);

    Task<List<ExpenseListDto>> GetFilteredExpensesAsync(
        string userId,
        int? groupId,
        string? searchTerm,
        int pageNumber,
        int pageSize);

    Task UpdateExpenseAsync(ExpenseUpdateDto dto);

    Task DeleteExpenseAsync(int id);
}