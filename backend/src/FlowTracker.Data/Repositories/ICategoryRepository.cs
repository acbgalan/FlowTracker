using FlowTracker.Data.Entities;
using FlowTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public interface ICategoryRepository : IRepositoryAsync<Category>
    {
        Task<bool> ExitsByNameAndTypeAsync(string name, TransactionType type);
        Task<Category?> GetAsync(int id, string userId);
        Task<List<Category>> GetAllAsync(string userId);
        Task<bool> IsCategoryValidForUserAsync(int id, string userId);
    }
}
