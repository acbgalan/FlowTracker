using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.Transaction;

namespace FlowTracker.Server.Services.Transaction
{
    public interface ITransactionService
    {
        Task<ServiceResult<TransactionResponse>> GetTransactionAsync(int id);
        Task<ServiceResult<List<TransactionResponse>>> GetTransactionsAsync();
        Task<ServiceResult<TransactionResponse>> CreateTransactionAsync(CreateTransactionRequest createTransactionRequest);
        Task<ServiceResult> UpdateTransactionAsync(UpdateTransactionRequest updateTransactionRequest);
        Task<ServiceResult> DeleteTransactionAsync(int id);
    }
}
