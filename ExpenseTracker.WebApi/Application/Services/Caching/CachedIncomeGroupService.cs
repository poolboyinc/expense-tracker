using ExpenseTracker.WebApi.Application.DTOs.IncomeGroup;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ExpenseTracker.WebApi.Application.Services.Caching;

public class CachedIncomeGroupService(
    IIncomeGroupService inner,
    IMemoryCache cache,
    IUserServiceContext userServiceContext) : IIncomeGroupService
{
    public async Task<List<IncomeGroupDto>> GetAllForUserAsync()
    {
        string key = GetCacheKey("all");

        return await cache.GetOrCreateAsync(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            return inner.GetAllForUserAsync();
        }) ?? [];
    }

    public async Task<IncomeGroupDto?> GetByIdAsync(int id)
    {
        string key = GetCacheKey($"id_{id}");

        return await cache.GetOrCreateAsync(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
            return inner.GetByIdAsync(id);
        });
    }

    public async Task<IncomeGroupDto> CreateAsync(IncomeGroupCreateDto dto)
    {
        var result = await inner.CreateAsync(dto);
     
        var key = GetCacheKey($"id_{result.Id}");
        
        cache.Set( key, result, TimeSpan.FromMinutes(15));
        
        InvalidateCache();

        return result;
    }

    public async Task UpdateAsync(int id, IncomeGroupUpdateDto dto)
    {
        await inner.UpdateAsync(id, dto);
        
        InvalidateCache(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await inner.DeleteAsync(id);

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
            cache.Remove(GetCacheKey($"id_{groupId.Value}"));
        }
    }

    private string GetCacheKey(string suffix)
    {
        return $"income_groups_{userServiceContext.GetCurrentUserId()}_{suffix}";
    }
}