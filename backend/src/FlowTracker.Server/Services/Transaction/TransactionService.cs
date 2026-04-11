using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Data.Repositories;
using FlowTracker.Server.Services.Category;
using FlowTracker.Server.Services.Common;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.Transaction;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FlowTracker.Server.Services.Transaction
{
    public class TransactionService : BaseService, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private string? _userId;

        public TransactionService(
            ITransactionRepository transactionRepository,
            ICurrentUserService currentUserService,
            ICategoryService categoryService,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<ServiceResult<TransactionResponse>> GetTransactionAsync(int id)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var transaction = await _transactionRepository.GetAsync(id, userId!);

                if (transaction == null)
                {
                    return FailureResult<TransactionResponse>("Transaction not found", StatusCodes.Status404NotFound);
                }

                var transactionResponse = _mapper.Map<TransactionResponse>(transaction);
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
                var userId = await GetUserIdCachedAsync();
                var transactions = await _transactionRepository.GetAllAsync(userId!);
                var transactionsResponse = _mapper.Map<List<TransactionResponse>>(transactions);

                return SuccessResult<List<TransactionResponse>>("Transactions retrieved successfully", StatusCodes.Status200OK, transactionsResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<List<TransactionResponse>>(ex);
            }
        }

        public async Task<ServiceResult<TransactionResponse>> CreateTransactionAsync(CreateTransactionRequest createTransactionRequest)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var isValidUserCategory = await _categoryService.IsCategoryValidForUserAsync(createTransactionRequest.CategoryId, userId!);

                if (!isValidUserCategory)
                {
                    return FailureResult<TransactionResponse>("Invalid category selection", StatusCodes.Status400BadRequest);
                }

                var transaction = _mapper.Map<FlowTracker.Data.Entities.Transaction>(createTransactionRequest);
                transaction.UserId = userId!;
                await _transactionRepository.AddAsync(transaction);
                await _transactionRepository.SaveAsync();
                var transactionResponse = _mapper.Map<TransactionResponse>(transaction);

                return SuccessResult<TransactionResponse>("Transaction created successfully", StatusCodes.Status201Created, transactionResponse);
            }
            catch (DbUpdateException ex)
            {
                return HandleDbUpdateException<TransactionResponse>(ex);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<TransactionResponse>(ex);
            }
        }

        public async Task<ServiceResult> UpdateTransactionAsync(UpdateTransactionRequest updateTransactionRequest)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var isValidUserCategory = await _categoryService.IsCategoryValidForUserAsync(updateTransactionRequest.CategoryId, userId!);

                if (!isValidUserCategory)
                {
                    return FailureResult("Invalid category selection", StatusCodes.Status400BadRequest);
                }

                var transaction = await _transactionRepository.GetAsync(updateTransactionRequest.Id, userId!);

                if (transaction == null)
                {
                    return FailureResult("Transaction not found", StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateTransactionRequest, transaction);
                await _transactionRepository.SaveAsync();
                return SuccessResult("Transaction updated successfully", StatusCodes.Status204NoContent);
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

        public async Task<ServiceResult> DeleteTransactionAsync(int id)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var transaction = await _transactionRepository.GetAsync(id, userId!);

                if (transaction == null)
                {
                    return FailureResult("Transaction not found", StatusCodes.Status404NotFound);
                }

                await _transactionRepository.DeleteAsync(transaction);
                await _transactionRepository.SaveAsync();

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

        private async Task<string?> GetUserIdCachedAsync()
        {
            if (_userId == null)
            {
                _userId = await _currentUserService.GetUserIdAsync();
            }

            return _userId;
        }

    }
}
