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
        var userCheck = await userRepository.GetUserByEmailAsync(request.Email);
        
        if (userCheck != null)
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
        byte[] salt = RandomNumberGenerator.GetBytes(16);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            100_000,                
            HashAlgorithmName.SHA256,
            32                       
        );
        
        byte[] combined = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, combined, salt.Length, hash.Length);

        return Convert.ToBase64String(combined);
    }
    
    
    private static bool VerifyPassword(string password, string storedHash)
    {
        byte[] combined = Convert.FromBase64String(storedHash);

        byte[] salt = new byte[16];
        byte[] storedHashBytes = new byte[32];

        Buffer.BlockCopy(combined, 0, salt, 0, 16);
        Buffer.BlockCopy(combined, 16, storedHashBytes, 0, 32);

        byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            100_000,
            HashAlgorithmName.SHA256,
            32
        );

        return CryptographicOperations.FixedTimeEquals(storedHashBytes, computedHash);
    }

}