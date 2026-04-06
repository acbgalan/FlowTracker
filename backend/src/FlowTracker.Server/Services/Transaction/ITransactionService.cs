using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.Transaction;

namespace FlowTracker.Server.Services.Transaction
{
    public interface ITransactionService
    {
        Task<ServiceResult<TransactionResponse>> GetTransaction(int id);
        Task<ServiceResult<TransactionResponse>> GetTransactionsAsync();
        Task<ServiceResult> CreateTransactionAsync();
        
    }
}
