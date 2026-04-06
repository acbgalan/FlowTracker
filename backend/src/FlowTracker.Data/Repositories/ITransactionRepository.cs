using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public interface ITransactionRepository : IRepositoryAsync<Transaction>
    {
        Task<List<Transaction>> GetAllAsync(string userId);
    }
}
