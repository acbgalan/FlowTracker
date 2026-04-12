using FlowTracker.Data.Contexts;
using FlowTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public class SavingGoalRepository : ISavingGoalRepository
    {
        private readonly ApplicationContext _context;

        public SavingGoalRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SavingGoal entity)
        {
            await _context.SavingGoals.AddAsync(entity);
        }

        public async Task<SavingGoal?> GetAsync(int id)
        {
            return await _context.SavingGoals.Include(x => x.SavingLogs).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SavingGoal?> GetAsync(int id, string userId)
        {
            return await _context.SavingGoals.Include(x => x.SavingLogs).FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        }

        public async Task<List<SavingGoal>> GetAllAsync()
        {
            return await _context.SavingGoals.Include(x => x.SavingLogs).ToListAsync();
        }

        public async Task<List<SavingGoal>> GetAllAsync(string userId)
        {
            return await _context.SavingGoals.Include(x => x.SavingLogs).Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task UpdateAsync(SavingGoal entity)
        {
            await Task.Run(() =>
            {
                _context.SavingGoals.Update(entity);
            });
        }

        public async Task DeleteAsync(int id)
        {
            var savingGoal = await GetAsync(id);

            if (savingGoal != null)
            {
                _context.SavingGoals.Remove(savingGoal);
            }
        }

        public async Task DeleteAsync(SavingGoal entity)
        {
            await Task.Run(() =>
            {
                _context.SavingGoals.Remove(entity);
            });
        }

        public async Task<bool> ExitsAsync(int id)
        {
            return await _context.SavingGoals.AnyAsync(x => x.Id == id);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
