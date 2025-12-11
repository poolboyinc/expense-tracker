using ExpenseTracker.WebApi.Application.DTOs.User;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(Guid id);

    Task<bool> UserExistsAsync(Guid userId);

    Task<UserDto> UpdateUserAsync(UserDto dto);

    Task<bool> DeleteUserAsync(Guid id);
}