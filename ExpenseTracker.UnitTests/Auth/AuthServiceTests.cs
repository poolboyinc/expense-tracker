using ExpenseTracker.WebApi.Application.DTOs.Auth;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Application.Services;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExpenseTracker.UnitTests.Auth;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<ITokenService> _tokenService = new();

    private AuthService Sut => new(
        _userRepo.Object,
        _tokenService.Object
    );
    
    [Fact]
    public async Task RegisterAsync_CreatesUser_AndReturnsToken()
    {
        var request = new RegisterRequest{ 
            Email = "test@test.com",
            Name = "Test User",
            Password = "password123"
        };

        _userRepo
            .Setup(r => r.GetUserByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);

        _userRepo
            .Setup(r => r.CreateUser(It.IsAny<User>()))
            .ReturnsAsync((User u) => u);

        _tokenService
            .Setup(t => t.CreateToken(It.IsAny<User>()))
            .Returns("fake-jwt");


        var response = await Sut.RegisterAsync(request);
        
        response.Token.Should().Be("fake-jwt");
        response.User.Email.Should().Be(request.Email);
        response.User.Name.Should().Be(request.Name);

        _userRepo.Verify(r =>
                r.CreateUser(It.Is<User>(u =>
                    u.Email == request.Email &&
                    u.IsPremium == false)),
            Times.Once);
    }
    
    [Fact]
    public async Task RegisterAsync_Throws_WhenEmailAlreadyExists()
    {
        var request = new RegisterRequest{ 
            Email = "test@test.com",
            Name = "Test",
            Password = "password"
        };

        _userRepo
            .Setup(r => r.GetUserByEmailAsync(request.Email))
            .ReturnsAsync(new User());

        Func<Task> act = async () =>
            await Sut.RegisterAsync(request);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User with this email already exists");
    }


    [Fact]
    public async Task LoginAsync_ReturnsToken_WhenCredentialsAreValid()
    {
        
        var registerRequest = new RegisterRequest{ 
            Email = "test@test.com",
            Name = "Test",
            Password = "password123"
        };

        User? createdUser = null;

        _userRepo
            .Setup(r => r.GetUserByEmailAsync(registerRequest.Email))
            .ReturnsAsync((User?)null);

        _userRepo
            .Setup(r => r.CreateUser(It.IsAny<User>()))
            .Callback<User>(u => createdUser = u)
            .ReturnsAsync((User u) => u);

        _tokenService
            .Setup(t => t.CreateToken(It.IsAny<User>()))
            .Returns("jwt");

        await Sut.RegisterAsync(registerRequest);

        _userRepo
            .Setup(r => r.GetUserByEmailAsync(registerRequest.Email))
            .ReturnsAsync(createdUser);

        var loginRequest = new LoginRequest{ 
            Email = registerRequest.Email,
            Password = registerRequest.Password
        };
        
        var response = await Sut.LoginAsync(loginRequest);
        
        response.Token.Should().Be("jwt");
    }

    [Fact]
    public async Task LoginAsync_Throws_WhenUserNotFound()
    {
        _userRepo
            .Setup(r => r.GetUserByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var request = new LoginRequest{Email = "no@test.com", Password = "pass"};

        Func<Task> act = async () =>
            await Sut.LoginAsync(request);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task LoginAsync_Throws_WhenPasswordIsWrong()
    {
        var registerRequest = new RegisterRequest{ 
            Email = "test@test.com",
            Name = "Test User",
            Password = "password123"
        };

        User? user = null;

        _userRepo
            .Setup(r => r.GetUserByEmailAsync(registerRequest.Email))
            .ReturnsAsync((User?)null);

        _userRepo
            .Setup(r => r.CreateUser(It.IsAny<User>()))
            .Callback<User>(u => user = u)
            .ReturnsAsync((User u) => u);

        _tokenService
            .Setup(t => t.CreateToken(It.IsAny<User>()))
            .Returns("jwt");

        await Sut.RegisterAsync(registerRequest);

        _userRepo
            .Setup(r => r.GetUserByEmailAsync(registerRequest.Email))
            .ReturnsAsync(user);

        var loginRequest = new LoginRequest{ 
            Email = registerRequest.Email,
            Password = "wrong-password"
        };

        Func<Task> act = async () =>
            await Sut.LoginAsync(loginRequest);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

}