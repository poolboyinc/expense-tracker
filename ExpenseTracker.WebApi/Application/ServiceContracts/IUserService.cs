using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.ServiceContracts;

public interface IUserService
{
    Task<string> GetCurrentUserIdAsync();
    
    Task<User?> GetUserByIdAsync(string id);
    

    Task<User> CreateUserAsync(User user);
    
    
    Task<bool> UserExistsAsync(string userId);
}