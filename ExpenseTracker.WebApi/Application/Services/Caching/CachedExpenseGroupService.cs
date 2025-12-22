using ExpenseTracker.WebApi.Application.DTOs.ExpenseGroup;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ExpenseTracker.WebApi.Application.Services.Caching;

public class CachedExpenseGroupService(
    IExpenseGroupService inner,           
    IMemoryCache cache, 
    IUserServiceContext userServiceContext) : IExpenseGroupService
{
    public async Task<List<ExpenseGroupListDto>> GetAllGroupsForUserAsync()
    {
        string key = GetCacheKey("all");

        return await cache.GetOrCreateAsync(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            return inner.GetAllGroupsForUserAsync();
        }) ?? [];
    }

    public async Task<ExpenseGroupDetailsDto?> GetGroupByIdAsync(int id)
    {
        string key = GetCacheKey($"id_{id}");

        return await cache.GetOrCreateAsync(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            return inner.GetGroupByIdAsync(id);
        });
    }

    public async Task<BudgetStatusDto> GetBudgetStatusAsync(int groupId)
    {
        string key = GetCacheKey($"budget_{groupId}");

        return await cache.GetOrCreateAsync(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            return inner.GetBudgetStatusAsync(groupId);
        }) ?? throw new KeyNotFoundException();
    }
    

    public async Task<ExpenseGroupDetailsDto> CreateGroupAsync(ExpenseGroupCreateDto dto)
    {
        var result = await inner.CreateGroupAsync(dto);

        string key = GetCacheKey($"id_{result.Id}");

        cache.Set(key, result, TimeSpan.FromMinutes(15));
        
        InvalidateCache(); 
        
        return result;
    }

    public async Task<ExpenseGroupDetailsDto> UpdateGroupAsync(int id, ExpenseGroupUpdateDto dto)
    {
        var result = await inner.UpdateGroupAsync(id, dto);
        
        string key = GetCacheKey($"id_{result.Id}");
        
        cache.Set(key, result, TimeSpan.FromMinutes(15));
        
        InvalidateCache(id);
        
        return result;
    }

    public async Task<bool> DeleteGroupAsync(int id)
    {
        var result = await inner.DeleteGroupAsync(id);
        
        if (result)
        {
            InvalidateCache(id);
        }

        return result;
    }

    private void InvalidateCache(int? groupId = null)
    {
        cache.Remove(GetCacheKey("all"));
        if (groupId.HasValue)
        {
            cache.Remove(GetCacheKey($"id_{groupId}"));
            cache.Remove(GetCacheKey($"budget_{groupId}"));
        }
    }
    
    public Task<decimal> GetTotalExpensesForGroupThisMonthAsync(int groupId) 
        => inner.GetTotalExpensesForGroupThisMonthAsync(groupId);

    public Task<decimal> GetTotalExpensesForGroupInRangeAsync(int groupId, DateTime from, DateTime to) 
        => inner.GetTotalExpensesForGroupInRangeAsync(groupId, from, to);
    
    private string GetCacheKey(string suffix) => 
        $"groups_{userServiceContext.GetCurrentUserId()}_{suffix}";
}