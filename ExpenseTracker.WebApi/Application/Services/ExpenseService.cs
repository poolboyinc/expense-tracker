using ExpenseTracker.WebApi.Application.DTOs.Expense;
using ExpenseTracker.WebApi.Application.Mappers;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Domain.Interfaces;

namespace ExpenseTracker.WebApi.Application.Services;

public class ExpenseService(IExpenseRepository expenseRepository, 
    IExpenseGroupRepository groupRepository,
    IUserServiceContext userServiceContext,
    IUserRepository userRepository,
    IEmailService emailService)
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
        var user = await userRepository.GetUserById(userId);

        if (user == null)
        {
            throw new InvalidOperationException($"User with ID {userId} not found.");
        }

        var expense = dto.ToEntity(userId);

        if (group == null)
        {
            throw new InvalidOperationException();
        }
        
        if (group.MonthlyLimit != null)
        {
            var currentTotal = await groupRepository
                .GetTotalExpensesForGroupThisMonthAsync(dto.ExpenseGroupId, userId);

            var newTotal = currentTotal + dto.Amount;

            if (newTotal > group.MonthlyLimit.Value)
            {
                throw new InvalidOperationException(
                    $"Monthly limit exceeded. Limit = {group.MonthlyLimit}, total = {newTotal}");
            }
           
            if (newTotal == group.MonthlyLimit.Value &&
                group.BudgetCapNotified == false &&
                user.IsPremium)
            {
                var subject = $"Budget Limit Reached for {group.Name}";
                var body = $@"
            <h2>Budget Cap Reached</h2>
            <p>You have reached your monthly budget cap for the group <strong>{group.Name}</strong>.</p>
            <p>Limit: <strong>{group.MonthlyLimit}</strong></p>
            <p>Total spent: <strong>{newTotal}</strong></p>
            <p>Keep tracking your expenses for better control!</p>";

                await emailService.SendEmailAsync(user.Email, subject, body);

                group.BudgetCapNotified = true;
                await groupRepository.UpdateGroupAsync(group);
            }
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
    
    var expenseGroupId = dto.ExpenseGroupId;
    
    if (existingExpense.ExpenseGroupId != dto.ExpenseGroupId)
    {
        var newGroup = await expenseRepository.GetGroupByIdAsync(dto.ExpenseGroupId);

        if (newGroup == null)
        {
            throw new InvalidOperationException($"Expense group with ID {dto.ExpenseGroupId} not found.");
        }
    }
    
    var group = await expenseRepository.GetGroupByIdAsync(expenseGroupId);
    
    if (group?.MonthlyLimit != null)
    {
        var currentTotal = await groupRepository
            .GetTotalExpensesForGroupThisMonthAsync(expenseGroupId, userId);
        
        var totalExcludingCurrentExpense = currentTotal - existingExpense.Amount;

        var newTotal = totalExcludingCurrentExpense + dto.Amount;

        if (newTotal > group.MonthlyLimit.Value)
        {
            throw new InvalidOperationException(
                $"Monthly limit exceeded for group '{group.Name}'. " +
                $"Limit = {group.MonthlyLimit}, " +
                $"Current Total (excluding this update) = {totalExcludingCurrentExpense}, " +
                $"New Total = {newTotal}");
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