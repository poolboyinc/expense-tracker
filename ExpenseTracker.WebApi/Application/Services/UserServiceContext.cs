using System.Security.Claims;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class UserServiceContext(IHttpContextAccessor httpContextAccessor) : IUserServiceContext
{
    public string GetCurrentUserId()
    {
        var httpContext = httpContextAccessor.HttpContext;
        
        if (httpContext?.User == null || !httpContext.User.Identity!.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User was not authenticated");
        }
        
        var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new InvalidOperationException("User id was not found in token");
        }

        return userId;
    }

    public bool IsAuthenticated()
    {
        return httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}