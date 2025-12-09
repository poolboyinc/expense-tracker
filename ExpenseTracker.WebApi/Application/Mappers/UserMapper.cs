using ExpenseTracker.WebApi.Application.DTOs.User;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user) =>
        new(user.Id, user.Email, user.Name);
}