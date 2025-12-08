using ExpenseTracker.WebApi.Domain.Entities;

namespace ExpenseTracker.WebApi.Application.ServiceContracts;

public interface ITokenService
{
    string CreateToken(User user);
}