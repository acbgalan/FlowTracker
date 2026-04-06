using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.Transaction;

namespace FlowTracker.Server.Services.Transaction
{
    public interface ITransactionService
    {
        Task<ServiceResult<TransactionResponse>> GetTransaction(int id);
        Task<ServiceResult<List<TransactionResponse>>> GetTransactionsAsync();
        Task<ServiceResult<List<TransactionResponse>>> GetTransactionsCurrentUserAsync();
        Task<ServiceResult> CreateTransactionAsync();

    }
}
