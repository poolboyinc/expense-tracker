using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    
    private const string FakeUserId = "user_12345"; 

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<string> GetCurrentUserIdAsync()
    {

        var exists = await _userRepository.UserExistsAsync(FakeUserId);

        if (!exists)
        {
            await _userRepository.CreateUser(new User 
            {
                Id = FakeUserId,
                Name = "Test User",
                IsPremium = false
            });
            return FakeUserId;
        }
        
        return FakeUserId;
    }
    
    public Task<User?> GetUserByIdAsync(string id)
    {
        return _userRepository.GetUserById(id);
    }
    
    public async Task<User> CreateUserAsync(User user)
    {
        var exists = await _userRepository.UserExistsAsync(user.Id);
        
        if (exists)
        {
            throw new InvalidOperationException($"User already exists");
        }
        
        user.IsPremium = false; 
        
        return await _userRepository.CreateUser(user);
    }
 
    public Task<bool> UserExistsAsync(string userId)
    {
        return _userRepository.UserExistsAsync(userId);
    }

    public Task<User> UpdateUserAsync(User user)
    {
        return  _userRepository.UpdateUser(user);
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return _userRepository.GetAllUsers();
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        //Plan: before deleting a user we have to delete his activity

        return await _userRepository.DeleteUserAsync(id);
    }
    
    public string GetCurrentUserId()
    {
        return FakeUserId;
    }
}