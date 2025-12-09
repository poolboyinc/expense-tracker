using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public Task<User?> GetUserByIdAsync(string id)
    {
        return userRepository.GetUserById(id);
    }
    

    public Task<bool> UserExistsAsync(string userId)
    {
        return userRepository.UserExistsAsync(userId);
    }

    public Task<User> UpdateUserAsync(User user)
    {
        return  userRepository.UpdateUser(user);
    }
    

    public async Task<bool> DeleteUserAsync(string id)
    {
        return await userRepository.DeleteUserAsync(id);
    }
    
}