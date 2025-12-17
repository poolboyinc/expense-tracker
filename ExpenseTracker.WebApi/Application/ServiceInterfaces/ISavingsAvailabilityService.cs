namespace ExpenseTracker.WebApi.Application.ServiceInterfaces;

public interface ISavingsAvailabilityService
{
    Task<bool> CanSpendAsync(Guid userId, decimal amount);
}
