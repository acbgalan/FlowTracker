using FlowTracker.Data.Contexts;
using FlowTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public interface ISavingGoalRepository : IRepositoryAsync<SavingGoal>
    {
        Task<SavingGoal?> GetAsync(int id, string userId);
        Task<List<SavingGoal>> GetAllAsync(string userId);
    }
}
