namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IUserServiceContext
{
    string GetCurrentUserId();
    
    bool IsAuthenticated();
}