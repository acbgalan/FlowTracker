using FlowTracker.Data.Contexts;
using FlowTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;
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
            return await _context.Transactions.FindAsync(id);

        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task UpdateAsync(Transaction entity)
        {
            await Task.Run(() =>
            {
                _context.Transactions.Update(entity);
            });
        }


        public async Task DeleteAsync(int id)
        {
            var transaction = await GetAsync(id);

            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
        }

        public async Task DeleteAsync(Transaction entity)
        {
            await Task.Run(() =>
            {
                _context.Transactions.Remove(entity);
            });
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
