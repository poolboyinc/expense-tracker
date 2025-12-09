using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    
    public Task<User?> GetUserByIdAsync(string id)
    {
        return _userRepository.GetUserById(id);
    }
    

    public Task<bool> UserExistsAsync(string userId)
    {
        return _userRepository.UserExistsAsync(userId);
    }

    public Task<User> UpdateUserAsync(User user)
    {
        return  _userRepository.UpdateUser(user);
    }
    

    public async Task<bool> DeleteUserAsync(string id)
    {
        return await _userRepository.DeleteUserAsync(id);
    }
    
}