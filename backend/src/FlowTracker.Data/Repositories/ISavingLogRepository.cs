using FlowTracker.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public interface ISavingLogRepository : IRepositoryAsync<SavingLog>
    {
        Task<SavingLog?> GetAsync(int id, string userId);
        Task<List<SavingLog>> GetAllAsync(string userId);
    }
}
