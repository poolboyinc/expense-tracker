using System.Security.Cryptography;
using System.Text;
using ExpenseTracker.WebApi.Application.DTOs.Auth;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class AuthService(IUserRepository userRepository, ITokenService tokenService) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.GetUserByEmailAsync(request.Email) != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }
        
        var passwordHash = HashPassword(request.Password);
        
        var user = new User
        {
            Email = request.Email,
            Name = request.Name,
            PasswordHash = passwordHash,
            IsPremium = false 
        };
        
        var createdUser = await userRepository.CreateUser(user);
        
        var token = tokenService.CreateToken(createdUser);
        
        var userDto = UserMapper.ToDto(createdUser);

        return new AuthResponse(
            userDto,
            token,
            DateTime.UtcNow.AddDays(7)
        );
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetUserByEmailAsync(request.Email);
        
        if (user == null)
        {
            throw new UnauthorizedAccessException("Email or password is incorrect");
        }
        
        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Email or password is incorrect");
        }
        
        var token = tokenService.CreateToken(user);
        
        var userDto = UserMapper.ToDto(user);

        return new AuthResponse(
            userDto,
            token,
            DateTime.UtcNow.AddDays(7)
        );
    }
    

    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }

    private static bool VerifyPassword(string password, string passwordHash)
    {
        var newHash = HashPassword(password);
        return newHash == passwordHash;
    }
}