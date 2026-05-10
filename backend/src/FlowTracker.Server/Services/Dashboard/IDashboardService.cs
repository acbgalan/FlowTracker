using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.Dashboard;

namespace FlowTracker.Server.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<ServiceResult<MonthlySummary>> GetMonthlySummary(DateOnly date);

        Task<ServiceResult<List<MonthlyCategoryExpenseSummary>>> GetMonthlyExpensesByCategory(DateOnly only);
    }
}
