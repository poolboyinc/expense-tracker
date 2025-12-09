using System.Security.Claims;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class UserServiceContext : IUserServiceContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserServiceContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
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
        return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}