using System.Security.Claims;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class UserServiceContext(IHttpContextAccessor httpContextAccessor) : IUserServiceContext
{
    public Guid GetCurrentUserId()
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext?.User == null || !httpContext.User.Identity!.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User was not authenticated");
        }

        var userIdString = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdString == null)
        {
            throw new InvalidOperationException("User id was not found in token");
        }

        if (!Guid.TryParse(userIdString, out var userId))
        {
            throw new InvalidOperationException("Invalid user id format in token");
        }

        return userId;
    }

    public bool IsAuthenticated()
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}