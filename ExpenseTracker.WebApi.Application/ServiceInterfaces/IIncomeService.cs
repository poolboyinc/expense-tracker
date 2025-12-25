using ExpenseTracker.WebApi.Application.DTOs.Income;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IIncomeService
{
    Task<IncomeDto> CreateIncomeAsync(IncomeCreateDto income);

    Task<IncomeDto?> GetIncomeByIdAsync(int id);

    Task<List<IncomeDto>> GetAllIncomesForUserAsync();

    Task UpdateIncomeAsync(int id, IncomeUpdateDto dto);

    Task<bool> DeleteIncomeAsync(int id);
}