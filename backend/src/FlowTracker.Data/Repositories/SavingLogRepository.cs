using FlowTracker.Data.Contexts;
using FlowTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public class SavingLogRepository : ISavingLogRepository
    {
        private readonly ApplicationContext _context;

        public SavingLogRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SavingLog entity)
        {
            await _context.SavingLogs.AddAsync(entity);
        }

        public async Task<SavingLog?> GetAsync(int id)
        {
            return await _context.SavingLogs.FindAsync(id);
        }

        public async Task<SavingLog?> GetAsync(int id, string userId)
        {
            return await _context.SavingLogs.FirstOrDefaultAsync(x => x.Id == id && x.SavingGoal.UserId == userId);
        }

        public async Task<List<SavingLog>> GetAllAsync()
        {
            return await _context.SavingLogs.ToListAsync();
        }

        public async Task<List<SavingLog>> GetAllAsync(string userId)
        {
            return await _context.SavingLogs.Where(x => x.SavingGoal.UserId == userId).ToListAsync();
        }

        public Task UpdateAsync(SavingLog entity)
        {
            _context.SavingLogs.Update(entity);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var savingLog = await GetAsync(id);

            if (savingLog != null)
            {
                _context.SavingLogs.Remove(savingLog);
            }
        }

        public Task DeleteAsync(SavingLog entity)
        {
            _context.SavingLogs.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> ExitsAsync(int id)
        {
            return await _context.SavingLogs.AnyAsync(x => x.Id == id);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }


    }
}
