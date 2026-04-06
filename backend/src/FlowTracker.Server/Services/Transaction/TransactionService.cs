using AutoMapper;
using FlowTracker.Data.Repositories;
using FlowTracker.Server.Services.Common;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.Transaction;
using Microsoft.EntityFrameworkCore;

namespace FlowTracker.Server.Services.Transaction
{
    public class TransactionService : BaseService, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly CurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, CurrentUserService currentUserService, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ServiceResult<TransactionResponse>> GetTransaction(int id)
        {
            try
            {
                var transaction = await _transactionRepository.GetAsync(id);
                var transactionResponse = transaction != null ? _mapper.Map<TransactionResponse>(transaction) : null;

                if (transaction == null)
                {
                    return FailureResult<TransactionResponse>("Transaction not found", StatusCodes.Status404NotFound);
                }

                return SuccessResult<TransactionResponse>("Transaction retrieved successfully", StatusCodes.Status200OK, transactionResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<TransactionResponse>(ex);
            }
        }

        public async Task<ServiceResult<List<TransactionResponse>>> GetTransactionsAsync()
        {
            try
            {
                var transactions = await _transactionRepository.GetAllAsync();
                var transactionsResponse = _mapper.Map<List<TransactionResponse>>(transactions);

                return SuccessResult<List<TransactionResponse>>("Transactions retrieved successfully", StatusCodes.Status200OK, transactionsResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<List<TransactionResponse>>(ex);
            }
        }

        public async Task<ServiceResult<List<TransactionResponse>>> GetTransactionsCurrentUserAsync()
        {
            try
            {
                var userId = await _currentUserService.GetUserIdAsync();
                var transactions = _transactionRepository.GetAllAsync(userId!);
                var transactionsResponse = _mapper.Map<List<TransactionResponse>>(transactions);

                return SuccessResult<List<TransactionResponse>>("Transactions retrieved successfully", StatusCodes.Status200OK, transactionsResponse);
            }
            catch (Exception ex)
            {
                {
                    return HandleGeneralException<List<TransactionResponse>>(ex);
                }
            }
        }

        public Task<ServiceResult> CreateTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> DeleteTransactionAsync(int id)
        {
            try
            {
                var transaction = await _transactionRepository.GetAsync(id);

                if (transaction == null)
                {
                    return FailureResult("Transaction not found", StatusCodes.Status404NotFound);
                }

                var userId = await _currentUserService.GetUserIdAsync();

                if (transaction.UserId != userId)
                {
                    return FailureResult("User is not allowed to delete the transaction", StatusCodes.Status403Forbidden);
                }

                await _transactionRepository.DeleteAsync(transaction);
                int saveResult = await _transactionRepository.SaveAsync();

                if (saveResult > 0)
                {
                    return FailureResult("Unexpected value when deleting transaction", StatusCodes.Status500InternalServerError);
                }

                return SuccessResult("Transaction deleted successfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException ex)
            {
                return HandleDbUpdateException(ex);
            }
            catch (Exception ex)
            {
                return HandleGeneralException(ex);
            }
        }

    }
}
