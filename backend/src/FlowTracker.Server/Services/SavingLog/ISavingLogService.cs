using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.SavingLog;

namespace FlowTracker.Server.Services.SavingLog
{
    public interface ISavingLogService
    {
        Task<ServiceResult<SavingLogResponse>> GetSavingLog(int id);
        Task<ServiceResult<List<SavingLogResponse>>> GetSavingLogs();
        Task<ServiceResult<SavingLogResponse>> CreateSavingLog(CreateSavingLogRequest createSavingLogRequest);
        Task<ServiceResult> UpdateSavingLog(UpdateSavingLogRequest updateSavingLogRequest);
        Task<ServiceResult> DeleteSavingLog(int id);
    }
}
