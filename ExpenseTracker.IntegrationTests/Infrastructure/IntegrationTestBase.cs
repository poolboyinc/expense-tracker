using ExpenseTracker.WebApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly ApplicationDbContext Context;
    protected readonly Guid TestUserId = Guid.NewGuid();

    protected IntegrationTestBase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(options);
        
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}