using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.SavingLog;

namespace FlowTracker.Server.Services.SavingLog
{
    public interface ISavingLogService
    {
        Task<ServiceResult<SavingLogResponse>> GetSavingLogAsync(int id);
        Task<ServiceResult<List<SavingLogResponse>>> GetSavingLogsAsync();
        Task<ServiceResult<SavingLogResponse>> CreateSavingLogAsync(CreateSavingLogRequest createSavingLogRequest);
        Task<ServiceResult> UpdateSavingLogAsync(UpdateSavingLogRequest updateSavingLogRequest);
        Task<ServiceResult> DeleteSavingLogAsync(int id);
    }
}
