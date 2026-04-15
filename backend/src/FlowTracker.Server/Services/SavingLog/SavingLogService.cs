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
        private string? _userId;

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

        public async Task<ServiceResult<SavingLogResponse>> GetSavingLogAsync(int id)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var savingLog = await _savingLogRepository.GetAsync(id, userId!);

                if (savingLog == null)
                {
                    return FailureResult<SavingLogResponse>("Saving log not found", StatusCodes.Status404NotFound);
                }

                var savingLogResponse = _mapper.Map<SavingLogResponse>(savingLog);
                return SuccessResult<SavingLogResponse>("Saving log retrieved successfully", StatusCodes.Status200OK, savingLogResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<SavingLogResponse>(ex);
            }
        }

        public async Task<ServiceResult<List<SavingLogResponse>>> GetSavingLogsAsync()
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var savingLogs = await _savingLogRepository.GetAllAsync(userId!);
                var savingLogsResponse = _mapper.Map<List<SavingLogResponse>>(savingLogs);

                return SuccessResult<List<SavingLogResponse>>("", StatusCodes.Status200OK, savingLogsResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<List<SavingLogResponse>>(ex);
            }
        }

        public async Task<ServiceResult<SavingLogResponse>> CreateSavingLogAsync(CreateSavingLogRequest createSavingLogRequest)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var savingGoalResult = await _savingGoalService.GetSavingGoalAsync(createSavingLogRequest.SavingGoalId);

                if (!savingGoalResult.Success)
                {
                    return FailureResult<SavingLogResponse>("Saving goal not found", StatusCodes.Status404NotFound);
                }

                var savingLog = _mapper.Map<Data.Entities.SavingLog>(createSavingLogRequest);
                await _savingLogRepository.AddAsync(savingLog);
                await _savingLogRepository.SaveAsync();
                var savingLogResponse = _mapper.Map<SavingLogResponse>(savingLog);

                return SuccessResult<SavingLogResponse>("Saving log created successfully", StatusCodes.Status201Created, savingLogResponse);
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

        public async Task<ServiceResult> UpdateSavingLogAsync(UpdateSavingLogRequest updateSavingLogRequest)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var savingGoalResult = await _savingGoalService.GetSavingGoalAsync(updateSavingLogRequest.SavingGoalId);

                if (!savingGoalResult.Success)
                {
                    return FailureResult("Saving goal not found", StatusCodes.Status404NotFound);
                }

                var savingLog = await _savingLogRepository.GetAsync(updateSavingLogRequest.Id, userId!);

                if (savingLog == null)
                {
                    return FailureResult("Saving log not found", StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateSavingLogRequest, savingLog);
                await _savingLogRepository.SaveAsync();
                return SuccessResult("Saving log updated successfully", StatusCodes.Status204NoContent);
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

        public async Task<ServiceResult> DeleteSavingLogAsync(int id)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var savingLog = await _savingLogRepository.GetAsync(id, userId!);

                if (savingLog == null)
                {
                    return FailureResult("Saving goal not found", StatusCodes.Status404NotFound);
                }

                await _savingLogRepository.DeleteAsync(savingLog);
                await _savingLogRepository.SaveAsync();

                return SuccessResult("Saving log deleted successfully", StatusCodes.Status204NoContent);
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
