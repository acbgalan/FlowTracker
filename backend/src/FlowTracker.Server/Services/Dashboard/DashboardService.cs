using FlowTracker.Data.Repositories;
using FlowTracker.Server.Services.Common;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.Dashboard;

namespace FlowTracker.Server.Services.Dashboard
{
    public class DashboardService : BaseService, IDashboardService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _currentUserService;
        private string? _userId;

        public DashboardService(ITransactionRepository transactionRepository, ICurrentUserService currentUserService)
        {
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ServiceResult<MonthlySummary>> GetMonthlySummary(DateOnly date)
        {
            try
            {
                if (date == default)
                {
                    return FailureResult<MonthlySummary>("Date is required", StatusCodes.Status400BadRequest);
                }

                var userId = await GetUserIdCachedAsync();
                var summary = await _transactionRepository.GetMonthlySummaryAsync(date, userId!);

                return SuccessResult("Monthly summary retrieved successfully", StatusCodes.Status200OK, summary);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<MonthlySummary>(ex);
            }
        }

        public async Task<ServiceResult<List<MonthlyCategoryExpenseSummary>>> GetMonthlyExpensesByCategory(DateOnly date)
        {
            try
            {
                if (date == default)
                {
                    return FailureResult<List<MonthlyCategoryExpenseSummary>>("Date is required", StatusCodes.Status400BadRequest);
                }

                var userId = await GetUserIdCachedAsync();
                var summary = await _transactionRepository.GetMonthlyExpensesByCategoryAsync(date, userId!);

                return SuccessResult("Monthly expenses by category retrieved successfully", StatusCodes.Status200OK, summary);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<List<MonthlyCategoryExpenseSummary>>(ex);
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
