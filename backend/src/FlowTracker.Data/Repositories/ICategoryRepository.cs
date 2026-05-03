using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public interface ICategoryRepository : IRepositoryAsync<Category>
    {
        Task<bool> ExitsByNameAndTypeAsync(string name, TransactionType type, string userId);
        Task<Category?> GetAsync(int id, string userId);
        Task<(List<Category> filteredCategories, int totalCount)> GetAllAsync(QueryParameters queryParameters, string userId);
        Task<bool> IsCategoryValidForUserAsync(int id, string userId);
    }
}
