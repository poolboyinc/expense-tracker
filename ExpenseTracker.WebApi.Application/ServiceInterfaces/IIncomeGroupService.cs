using ExpenseTracker.WebApi.Application.DTOs.IncomeGroup;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IIncomeGroupService
{
    Task<IncomeGroupDto> CreateAsync(IncomeGroupCreateDto dto);
    Task<IncomeGroupDto?> GetByIdAsync(int id);
    Task<List<IncomeGroupDto>> GetAllForUserAsync();
    Task UpdateAsync(int id, IncomeGroupUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}