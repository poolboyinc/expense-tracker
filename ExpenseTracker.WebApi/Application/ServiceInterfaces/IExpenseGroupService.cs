using ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IExpenseGroupService
{
    Task<List<ExpenseGroupListDto>> GetAllGroupsForUserAsync();
    
    Task<ExpenseGroupDetailsDto?> GetGroupByIdAsync(int id);
    
    Task<ExpenseGroupDetailsDto> CreateGroupAsync(ExpenseGroupCreateDto dto);
    
    Task<ExpenseGroupDetailsDto> UpdateGroupAsync(int id, ExpenseGroupUpdateDto dto);
    
    Task<bool> DeleteGroupAsync(int id);
}