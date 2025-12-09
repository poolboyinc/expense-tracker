using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IUserService
{
    
    Task<User?> GetUserByIdAsync(string id);

    Task<bool> UserExistsAsync(string userId);

    Task<User> UpdateUserAsync(User user);
    
    Task<bool> DeleteUserAsync(string id);
    
}