using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Data.Repositories;
using FlowTracker.Server.Services.Category;
using FlowTracker.Server.Services.SavingGoal;
using FlowTracker.Server.Services.Common;
using FlowTracker.Shared.Dtos.Category;
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
        private readonly ISavingGoalService _savingGoalService;
        private readonly IMapper _mapper;
        private string? _userId;
        private readonly HashSet<string> validSortFields = new HashSet<string>() { "id", "date", "type", "category", "amount", "description" };

        public TransactionService(
            ITransactionRepository transactionRepository,
            ICurrentUserService currentUserService,
            ICategoryService categoryService,
            ISavingGoalService savingGoalService,
            IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;
            _categoryService = categoryService;
            _savingGoalService = savingGoalService;
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

        public async Task<ServiceResult<PagedResponse<TransactionResponse>>> GetTransactionsAsync(QueryParameters queryParameters)
        {
            try
            {
                //Move to Fluent Validation
                //Sort validation
                if (!string.IsNullOrWhiteSpace(queryParameters.SortBy) && !validSortFields.Contains(queryParameters.SortBy))
                {
                    return FailureResult<PagedResponse<TransactionResponse>>("Bad parameter", StatusCodes.Status400BadRequest);

                }

                //Page and limit validation
                if (queryParameters.Page < 1 || queryParameters.Limit < 1)
                {
                    return FailureResult<PagedResponse<TransactionResponse>>("Bad paramater", StatusCodes.Status400BadRequest);
                }

                var userId = await GetUserIdCachedAsync();
                var (filteredTransactions, totalCount) = await _transactionRepository.GetAllAsync(queryParameters, userId!);

                var pagedResponse = new PagedResponse<TransactionResponse>
                {
                    Data = _mapper.Map<List<TransactionResponse>>(filteredTransactions),
                    Page = queryParameters.Page,
                    Limit = queryParameters.Limit,
                    Total = totalCount
                };

                return SuccessResult<PagedResponse<TransactionResponse>>("Transactions retrieved successfully", StatusCodes.Status200OK, pagedResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<PagedResponse<TransactionResponse>>(ex);
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

                if (createTransactionRequest.SavingGoalId.HasValue)
                {
                    var savingGoal = await _savingGoalService.GetSavingGoalAsync(createTransactionRequest.SavingGoalId.Value);

                    if (!savingGoal.Success)
                    {
                        return FailureResult<TransactionResponse>("Saving goal not found", StatusCodes.Status404NotFound);
                    }
                }

                var transaction = _mapper.Map<FlowTracker.Data.Entities.Transaction>(createTransactionRequest);
                transaction.UserId = userId!;
                await _transactionRepository.AddAsync(transaction);
                await _transactionRepository.SaveAsync();

                var savedTransaction = await _transactionRepository.GetAsync(transaction.Id, userId!);
                var transactionResponse = _mapper.Map<TransactionResponse>(savedTransaction);

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

                if (updateTransactionRequest.SavingGoalId.HasValue)
                {
                    var savingGoal = await _savingGoalService.GetSavingGoalAsync(updateTransactionRequest.SavingGoalId.Value);

                    if (!savingGoal.Success)
                    {
                        return FailureResult("Saving goal not found", StatusCodes.Status404NotFound);
                    }
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

                return SuccessResult("Transaction deleted successfully", StatusCodes.Status204NoContent);
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
