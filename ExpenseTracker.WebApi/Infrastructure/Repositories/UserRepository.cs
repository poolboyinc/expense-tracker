using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetUserById(string id)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> CreateUser(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        context.Users.Update(user);
        
        await context.SaveChangesAsync(); 
        
        return user;
    }

    public async Task<bool> UserExistsAsync(string userId)
    {
        return await context.Users.AnyAsync(x => x.Id == userId);
    }

    //Test endpoint - will be removed when done with every test
    public async Task<List<User>> GetAllUsers()
    {
        var users = await context.Users.ToListAsync();
        
        return users;
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var userToDelete = await context.Users.FindAsync(id);

        if (userToDelete == null)
        {
            return false; 
        }

        context.Users.Remove(userToDelete);
        
        await context.SaveChangesAsync();
        
        return true; 
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}