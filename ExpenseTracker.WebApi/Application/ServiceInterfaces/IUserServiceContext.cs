namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface IUserServiceContext
{
    Guid GetCurrentUserId();

    bool IsAuthenticated();
}