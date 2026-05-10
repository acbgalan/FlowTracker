using FlowTracker.Data.Contexts;
using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.Dashboard;
using FlowTracker.Shared.Dtos.Transaction;
using FlowTracker.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationContext _context;

        public TransactionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction entity)
        {
            await _context.Transactions.AddAsync(entity);
        }

        public async Task<Transaction?> GetAsync(int id)
        {
            return await _context.Transactions.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Transaction?> GetAsync(int id, string userId)
        {
            return await _context.Transactions.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.Include(x => x.Category).ToListAsync();
        }

        public async Task<(List<Transaction> filteredTransactions, int totalCount)> GetAllAsync(QueryParameters queryParameters, string userId)
        {
            IQueryable<Transaction> filteredTransactions = _context.Transactions;

            //UserId filtering
            filteredTransactions = filteredTransactions.Where(x => x.UserId == userId);

            //SearchTerm filtering
            if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                string searchTerm = queryParameters.SearchTerm.ToLower();

                filteredTransactions = filteredTransactions.Where(x =>
                x.Category.Type.ToString().ToLower().Contains(searchTerm) ||
                x.Category.Name.ToLower().Contains(searchTerm) ||
                x.Amount.ToString().ToLower().Contains(searchTerm) ||
                x.Description.ToLower().Contains(searchTerm));
            }

            int totalCount = await filteredTransactions.CountAsync();

            if (!string.IsNullOrWhiteSpace(queryParameters.SortBy))
            {
                switch (queryParameters.SortBy.ToLower())
                {
                    case "date":
                        filteredTransactions = queryParameters.SortDesc ? filteredTransactions.OrderByDescending(x => x.Date) : filteredTransactions.OrderBy(x => x.Date);
                        break;
                    case "type":
                        filteredTransactions = queryParameters.SortDesc ? filteredTransactions.OrderByDescending(x => x.Category.Type) : filteredTransactions.OrderBy(x => x.Category.Type);
                        break;
                    case "category":
                        filteredTransactions = queryParameters.SortDesc ? filteredTransactions.OrderByDescending(x => x.Category.Name) : filteredTransactions.OrderBy(x => x.Category.Name);
                        break;
                    case "amount":
                        filteredTransactions = queryParameters.SortDesc ? filteredTransactions.OrderByDescending(x => x.Amount) : filteredTransactions.OrderBy(x => x.Amount);
                        break;
                    case "description":
                        filteredTransactions = queryParameters.SortDesc ? filteredTransactions.OrderByDescending(x => x.Description) : filteredTransactions.OrderBy(x => x.Description);
                        break;
                    default:
                        filteredTransactions = queryParameters.SortDesc ? filteredTransactions.OrderByDescending(x => x.Id) : filteredTransactions.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                filteredTransactions = queryParameters.SortDesc ? filteredTransactions.OrderByDescending(x => x.Id) : filteredTransactions.OrderBy(x => x.Id);
            }

            //Pagination
            int skip = (queryParameters.Page - 1) * queryParameters.Limit;
            filteredTransactions = filteredTransactions.Skip(skip).Take(queryParameters.Limit);

            return (await filteredTransactions.Include(x => x.Category).ToListAsync(), totalCount);
        }

        public async Task<MonthlySummary> GetMonthlySummaryAsync(DateOnly date, string userId)
        {
            var monthStart = new DateOnly(date.Year, date.Month, 1);
            var monthEnd = monthStart.AddMonths(1);

            var transactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(x => x.UserId == userId && x.Date >= monthStart && x.Date < monthEnd)
                .ToListAsync();

            var income = transactions
                .Where(x => x.Category.Type == TransactionType.Income)
                .Sum(x => x.Amount);

            var expense = transactions
                .Where(x => x.Category.Type == TransactionType.Expense)
                .Sum(x => x.Amount);

            var saving = transactions
                .Where(x => x.Category.Type == TransactionType.Saving)
                .Sum(x => x.Amount);

            return new MonthlySummary
            {
                Year = date.Year,
                Month = date.Month,
                Income = income,
                Expense = expense,
                Saving = saving,
                Balance = income - (expense + saving)
            };
        }

        public async Task<List<MonthlyCategoryExpenseSummary>> GetMonthlyExpensesByCategoryAsync(DateOnly date, string userId)
        {
            var monthStart = new DateOnly(date.Year, date.Month, 1);
            var monthEnd = monthStart.AddMonths(1);

            return await _context.Transactions
                .Include(x => x.Category)
                .Where(x => x.UserId == userId && x.Date >= monthStart && x.Date < monthEnd)
                .Where(x => x.Category.Type == TransactionType.Expense)
                .GroupBy(x => x.Category.Name)
                .Select(g => new MonthlyCategoryExpenseSummary
                {
                    CategoryName = g.Key,
                    Amount = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.CategoryName)
                .ToListAsync();
        }

        public Task UpdateAsync(Transaction entity)
        {
            _context.Transactions.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await GetAsync(id);

            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
        }

        public Task DeleteAsync(Transaction entity)
        {
            _context.Transactions.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> ExitsAsync(int id)
        {
            return await _context.Transactions.AnyAsync(t => t.Id == id);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
