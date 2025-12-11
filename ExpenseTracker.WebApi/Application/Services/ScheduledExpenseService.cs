using ExpenseTracker.WebApi.Application.DTOs.ScheduledExpense;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class ScheduledExpenseService(
    IScheduledExpenseRepository scheduledExpenseRepository,
    IExpenseRepository expenseRepository,
    IExpenseGroupRepository groupRepository,
    IUserServiceContext userServiceContext
) : IScheduledExpenseService
{
    public async Task<ScheduledExpenseDto> CreateAsync(ScheduledExpenseCreateDto dto)
    {
        var userId = userServiceContext.GetCurrentUserId();

        var group = await groupRepository.GetGroupByIdAsync(dto.ExpenseGroupId, userId);
        if (group == null)
        {
            throw new InvalidOperationException("Expense group not found.");
        }

        var entity = dto.ToEntity(userId);
        var created = await scheduledExpenseRepository.CreateAsync(entity);
        return created.ToDto();
    }

    public async Task<List<ScheduledExpenseDto>> GetAllForCurrentUserAsync()
    {
        var userId = userServiceContext.GetCurrentUserId();
        var list = await scheduledExpenseRepository.GetAllForUserAsync(userId);
        return list.Select(x => x.ToDto()).ToList();
    }

    public async Task<ScheduledExpenseDto?> GetByIdAsync(int id)
    {
        var item = await scheduledExpenseRepository.GetByIdAsync(id);
        if (item == null)
        {
            return null;
        }

        var userId = userServiceContext.GetCurrentUserId();
        if (item.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        return item.ToDto();
    }

    public async Task UpdateAsync(ScheduledExpenseUpdateDto dto)
    {
        var userId = userServiceContext.GetCurrentUserId();
        var existing = await scheduledExpenseRepository.GetByIdAsync(dto.Id);
        if (existing == null)
        {
            throw new KeyNotFoundException("Not found.");
        }

        if (existing.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        var group = await groupRepository.GetGroupByIdAsync(dto.ExpenseGroupId, userId);
        if (group == null)
        {
            throw new InvalidOperationException("Expense group not found.");
        }

        dto.MapUpdateToEntity(existing);
        await scheduledExpenseRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var userId = userServiceContext.GetCurrentUserId();
        var existing = await scheduledExpenseRepository.GetByIdAsync(id);
        if (existing == null)
        {
            throw new KeyNotFoundException();
        }

        if (existing.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        await scheduledExpenseRepository.DeleteAsync(existing);
    }

    public async Task<int> ProcessDueScheduledExpensesAsync(DateTime utcNow)
    {
        var due = await scheduledExpenseRepository.GetDueScheduledExpensesAsync(utcNow);
        var createdCount = 0;

        foreach (var s in due)
        {
            try
            {
                var expense = new Expense
                {
                    Amount = s.Amount,
                    Description = s.Description,
                    TransactionDate = s.NextRunAt,
                    ExpenseGroupId = s.ExpenseGroupId,
                    UserId = s.UserId,
                    IsScheduled = true
                };

                await expenseRepository.AddAsync(expense);
                createdCount++;

                var next = RecurrenceCalculator.ComputeNextRun(s);
                if (next == null || (s.EndAt.HasValue && next > s.EndAt.Value))
                {
                    s.IsActive = false;
                }
                else
                {
                    s.NextRunAt = next.Value;
                }

                await scheduledExpenseRepository.UpdateAsync(s);
            }
            catch
            {
            }
        }

        return createdCount;
    }
    
}