using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Data.Repositories;
using FlowTracker.Server.Services.Common;
using FlowTracker.Server.Services.SavingGoal;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.SavingLog;
using Microsoft.EntityFrameworkCore;

namespace FlowTracker.Server.Services.SavingLog
{
    public class SavingLogService : BaseService, ISavingLogService
    {
        private readonly ISavingLogRepository _savingLogRepository;
        private readonly ISavingGoalService _savingGoalService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public SavingLogService(
            ISavingLogRepository savingLogRepository,
            ISavingGoalService savingGoalService,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _savingLogRepository = savingLogRepository;
            _savingGoalService = savingGoalService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }


        public async Task<ServiceResult<SavingLogResponse>> CreateSavingLog(CreateSavingLogRequest createSavingLogRequest)
        {
            try
            {
                var userId = await _currentUserService.GetUserIdAsync();
                var savingGoalResult = await _savingGoalService.GetSavingGoalAsync(createSavingLogRequest.SavingGoalId);

                if (!savingGoalResult.Success)
                {
                    return FailureResult<SavingLogResponse>("Saving log not found", StatusCodes.Status404NotFound);
                }

                var savingLog = _mapper.Map<Data.Entities.SavingLog>(createSavingLogRequest);
                await _savingLogRepository.AddAsync(savingLog);
                await _savingLogRepository.SaveAsync();
                var savingLogResponse = _mapper.Map<SavingLogResponse>(savingLog);

                return SuccessResult<SavingLogResponse>("Saving log created successfully", StatusCodes.Status200OK, savingLogResponse);
            }
            catch (DbUpdateException ex)
            {
                return HandleDbUpdateException<SavingLogResponse>(ex);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<SavingLogResponse>(ex);
            }
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
