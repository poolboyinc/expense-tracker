using System.Text;
using ExpenseTracker.WebApi.Application.ServiceInterfaces;
using ExpenseTracker.WebApi.Application.Services;
using ExpenseTracker.WebApi.Domain.Interfaces;
using ExpenseTracker.WebApi.Infrastructure.Configuration;
using ExpenseTracker.WebApi.Infrastructure.HostedServices;
using ExpenseTracker.WebApi.Infrastructure.Persistence;
using ExpenseTracker.WebApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
        .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();

builder.Services.AddScoped<IExpenseGroupRepository, ExpenseGroupRepository>();

builder.Services.AddScoped<ISavingsPlanRepository, SavingsPlanRepository>();

builder.Services.AddScoped<IExpenseService, ExpenseService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IExpenseGroupService, ExpenseGroupService>();

builder.Services.AddScoped<IIncomeService, IncomeService>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ISavingsPlanService, SavingsPlanService>();

builder.Services.AddScoped<ISavingsPlanCalculator, SavingsPlanCalculator>();

builder.Services.AddScoped<ISavingsAvailabilityService, SavingsAvailabilityService>();

builder.Services.Configure<SavingsPlanWorkerOptions>(
    builder.Configuration.GetSection("SavingsPlanWorker"));

builder.Services.AddHostedService<MonthlyBudgetResetWorker>();

builder.Services.AddHostedService<SavingsPlanWorker>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserServiceContext, UserServiceContext>();

var tokenKey = builder.Configuration["Token:Key"]
               ?? throw new InvalidOperationException("The token was not set in configuration file.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Premium", policy =>
        policy.RequireClaim("is_premium", "true"));
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();
//builder.Services.AddSwaggerGen();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();