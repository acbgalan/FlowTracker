using AutoMapper;
using FlowTracker.Data.Repositories;
using FlowTracker.Server.Services.Common;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.Transaction;

namespace FlowTracker.Server.Services.Transaction
{
    public class TransactionService : BaseService, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }


        public Task<ServiceResult> CreateTransactionAsync()
        {
            throw new NotImplementedException();
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


    }
}
