using ExpenseTracker.WebApi.Domain.Entities;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.WebApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    
    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetUserById(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        _context.Users.Update(user);
        
        await _context.SaveChangesAsync(); 
        
        return user;
    }

    public async Task<bool> UserExistsAsync(string userId)
    {
        return await _context.Users.AnyAsync(x => x.Id == userId);
    }

    //Test endpoint - will be removed when done with every test
    public async Task<List<User>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        
        return users;
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var userToDelete = await _context.Users.FindAsync(id);

        if (userToDelete == null)
        {
            return false; 
        }

        _context.Users.Remove(userToDelete);
        
        await _context.SaveChangesAsync();
        
        return true; 
    }
}