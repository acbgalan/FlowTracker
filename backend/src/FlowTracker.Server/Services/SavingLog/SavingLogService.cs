using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.SavingLog;

namespace FlowTracker.Server.Services.SavingLog
{
    public class SavingLogService : ISavingLogService
    {
        public Task<ServiceResult<SavingLogResponse>> CreateSavingLog(CreateSavingLogRequest createSavingLogRequest)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> DeleteSavingLog(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<SavingLogResponse>> GetSavingLog(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<List<SavingLogResponse>>> GetSavingLogs()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult> UpdateSavingLog(UpdateSavingLogRequest updateSavingLogRequest)
        {
            throw new NotImplementedException();
        }
    }
}
