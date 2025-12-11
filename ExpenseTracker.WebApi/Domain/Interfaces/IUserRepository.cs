using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserById(Guid id);
    Task<User> CreateUser(User user);
    Task<User> UpdateUser(User user);
    Task<bool> UserExistsAsync(Guid userId);
    Task<bool> DeleteUserAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);
}