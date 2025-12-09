using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IUserService
{
    Task<string> GetCurrentUserIdAsync();
    
    Task<User?> GetUserByIdAsync(string id);
    

    Task<User> CreateUserAsync(User user);
    
    
    Task<bool> UserExistsAsync(string userId);
    
    Task<User> UpdateUserAsync(User user);
    
    Task<List<User>> GetAllUsersAsync();
    
    Task<bool> DeleteUserAsync(string id);
    
    string GetCurrentUserId();
}