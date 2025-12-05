using ExpenseTracker.WebApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseTracker.WebApi.Domain.Interfaces;

public interface IIncomeRepository
{
   Task<Income> CreateIncomeAsync(Income income);

   Task<Income?> GetIncomeByIdAsync(int id);

   Task<List<Income>> GetAllIncomesByUserIdAsync(string userId);

   Task<Income> UpdateIncomeAsync(Income income);

   Task<bool> DeleteIncomeAsync(int id);
} 
