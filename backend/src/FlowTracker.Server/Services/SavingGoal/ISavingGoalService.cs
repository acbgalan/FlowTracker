using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.SavingGoal;

namespace FlowTracker.Server.Services.SavingGoal
{
    public interface ISavingGoalService
    {
        Task<ServiceResult<SavingGoalResponse>> GetSavingGoalAsync(int id);
    }
}
