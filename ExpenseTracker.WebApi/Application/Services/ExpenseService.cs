using ExpenseTracker.WebApi.Application.DTOs.Expense;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class ExpenseService(IExpenseRepository expenseRepository, IUserServiceContext userServiceContext)
    : IExpenseService
{
    public async Task<ExpenseDetailsDto?> GetExpenseByIdAsync(int id)
    {
        var userId = userServiceContext.GetCurrentUserId();

        var expense = await expenseRepository.GetByIdAsync(id, userId);

        if (expense == null)
        {
            throw new InvalidOperationException();
        }

        return expense.ToDetailsDto();
    }

    public async Task<ExpenseDetailsDto> CreateExpenseAsync(ExpenseCreateDto dto)
    {
        var group = await expenseRepository.GetGroupByIdAsync(dto.ExpenseGroupId);

        if (group == null)
        {
            throw new InvalidOperationException($"Expense group with ID {dto.ExpenseGroupId} not found.");
        }

        var userId = userServiceContext.GetCurrentUserId();

        var expense = dto.ToEntity(userId);

        if (group == null)
        {
            throw new InvalidOperationException();
        }


        await expenseRepository.AddAsync(expense);

        return expense.ToDetailsDto();
    }

    public async Task<List<ExpenseListDto>> GetFilteredExpensesAsync(Guid userId, int? groupId, string? searchTerm,
        int pageNumber, int pageSize)
    {
        var expenses = await expenseRepository.GetExpensesAsync(userId, groupId, searchTerm, pageNumber, pageSize);

        var expenseListDtos = expenses.Select(expense => expense.ToListDto()).ToList();

        return expenseListDtos;
    }

    public async Task UpdateExpenseAsync(ExpenseUpdateDto dto)
    {
        var userId = userServiceContext.GetCurrentUserId();

        var existingExpense = await expenseRepository.GetByIdAsync(dto.Id, userId);

        if (existingExpense == null || existingExpense.UserId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        if (existingExpense.ExpenseGroupId != dto.ExpenseGroupId)
        {
            var group = await expenseRepository.GetGroupByIdAsync(dto.ExpenseGroupId);

            if (group == null)
            {
                throw new InvalidOperationException();
            }
        }

        dto.MapUpdateToEntity(existingExpense);

        await expenseRepository.UpdateAsync(existingExpense);
    }


    public async Task DeleteExpenseAsync(int id)
    {
        var userId = userServiceContext.GetCurrentUserId();
        var expense = await expenseRepository.GetByIdAsync(id, userId);

        if (expense == null)
        {
            throw new InvalidOperationException();
        }

        await expenseRepository.DeleteAsync(expense);
    }
}