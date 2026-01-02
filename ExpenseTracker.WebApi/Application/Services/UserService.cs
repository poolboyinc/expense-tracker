using ExpenseTracker.WebApi.Application.DTOs.User;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class UserService(IUserRepository userRepository, IUserServiceContext userServiceContext, ITokenService tokenService) : IUserService
{
    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var userEntity = await userRepository.GetUserById(id);

        if (userEntity == null)
        {
            return null;
        }

        return userEntity.ToDto();
    }


    public Task<bool> UserExistsAsync(Guid userId)
    {
        return userRepository.UserExistsAsync(userId);
    }

    public async Task<UserDto> UpdateUserAsync(UserDto dto)
    {
        var existingUser = await userRepository.GetUserById(dto.Id);

        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with ID {dto.Id} not found.");
        }

        existingUser.Email = dto.Email;
        existingUser.Name = dto.Name;

        var updatedEntity = await userRepository.UpdateUser(existingUser);

        return updatedEntity.ToDto();
    }
    
    public async Task<UpgradeToPremiumDto> UpgradeToPremiumAsync()
    {
        var userId = userServiceContext.GetCurrentUserId();
        var user = await userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (user.IsPremium)
        {
            throw new InvalidOperationException("User is already premium.");
        }

        user.IsPremium = true;
        await userRepository.UpdateUser(user);
        
        var newToken = tokenService.CreateToken(user);

        return new UpgradeToPremiumDto(
            newToken,
            user.IsPremium
        );
    }
    
    public async Task DowngradeFromPremiumAsync()
    {
        var userId = userServiceContext.GetCurrentUserId();
        var user = await userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        user.IsPremium = false;
        await userRepository.UpdateUser(user);
    }



    public async Task<bool> DeleteUserAsync(Guid id)
    {
        return await userRepository.DeleteUserAsync(id);
    }
}