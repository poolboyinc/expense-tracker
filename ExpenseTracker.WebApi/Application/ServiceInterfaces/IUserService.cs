using ExpenseTracker.WebApi.Application.DTOs.User;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IUserService
{
    
    Task<UserDto?> GetUserByIdAsync(string id);

    Task<bool> UserExistsAsync(string userId);

    Task<UserDto> UpdateUserAsync(UserDto dto);
    
    Task<bool> DeleteUserAsync(string id);
    
}