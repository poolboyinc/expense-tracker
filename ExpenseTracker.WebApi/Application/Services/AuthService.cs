using System.Security.Cryptography;
using System.Text;
using ExpenseTracker.WebApi.Application.DTOs.Auth;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class AuthService : IAuthService
{
     private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    
    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.GetUserByEmailAsync(request.Email) != null)
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
        
        var createdUser = await _userRepository.CreateUser(user);
        
        var token = _tokenService.CreateToken(createdUser);
        
        return new AuthResponse
        {
            UserId = createdUser.Id,
            UserName = createdUser.Name,
            Token = token,
            ExpiresAt = DateTime.Now.AddDays(7) 
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        
        if (user == null)
        {
            throw new UnauthorizedAccessException("Email or password is incorrect");
        }
        
        if (!VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Email or password is incorrect");
        }
        
        var token = _tokenService.CreateToken(user);
        
        return new AuthResponse
        {
            UserId = user.Id,
            UserName = user.Name,
            Token = token,
            ExpiresAt = DateTime.Now.AddDays(7)
        };
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