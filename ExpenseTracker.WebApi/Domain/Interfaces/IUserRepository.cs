using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserById(string id);
    Task<User> CreateUser(User user);
    Task<User>  UpdateUser(User user);
    Task<bool> UserExistsAsync(string userId);
    Task<List<User>> GetAllUsers();
    Task<bool> DeleteUserAsync(string id);
    Task<User?> GetUserByEmailAsync(string email);
}