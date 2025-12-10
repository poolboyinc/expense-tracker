using ExpenseTracker.WebApi.Application.DTOs.User;
using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.Email,
            user.Name
        );
    }

    public static User ToEntity(this UserDto dto)
    {
        return new User()
        {
            Id = dto.Id,
            Email = dto.Email,
            Name = dto.Name
        };
    }
}