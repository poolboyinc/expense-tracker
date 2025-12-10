using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetUserById(string id)
    {
        return await context.User.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> CreateUser(User user)
    {
        await context.User.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        context.User.Update(user);
        
        await context.SaveChangesAsync(); 
        
        return user;
    }

    public async Task<bool> UserExistsAsync(string userId)
    {
        return await context.User.AnyAsync(x => x.Id == userId);
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var userToDelete = await context.User.FindAsync(id);

        if (userToDelete == null)
        {
            return false; 
        }

        context.User.Remove(userToDelete);
        
        await context.SaveChangesAsync();
        
        return true; 
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.User.FirstOrDefaultAsync(u => u.Email == email);
    }
}