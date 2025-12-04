using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetUserById(string id);
    public Task<User> CreateUser(User user);
    Task<bool> UserExistsAsync(string userId);
}