using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.Dashboard;
using FlowTracker.Shared.Dtos.Transaction;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public interface ITransactionRepository : IRepositoryAsync<Transaction>
    {
        Task<Transaction?> GetAsync(int id, string userId);
        Task<(List<Transaction> filteredTransactions, int totalCount )> GetAllAsync(QueryParameters queryParameters, string userId);
        Task<MonthlySummary> GetMonthlySummaryAsync(DateOnly date, string userId);
        Task<List<MonthlyCategoryExpenseSummary>> GetMonthlyExpensesByCategoryAsync(DateOnly date, string userId);
    }
}
