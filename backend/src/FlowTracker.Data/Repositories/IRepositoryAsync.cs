using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public interface IRepositoryAsync<T>
    {
        Task AddAsync(T entity);
        Task<T?> GetAsync(int id);
        Task<List<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task DeleteASync(T entity);
        Task<bool> ExitsAsync(int id);
        Task<int> SaveAsync();
    }
}
