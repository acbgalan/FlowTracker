using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Data.Repositories;
using FlowTracker.Server.Services.Common;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.SavingGoal;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FlowTracker.Server.Services.SavingGoal
{
    public class SavingGoalService : BaseService, ISavingGoalService
    {
        private readonly ISavingGoalRepository _savingGoalRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private string? _userId;

        public SavingGoalService(ISavingGoalRepository savingGoalRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _savingGoalRepository = savingGoalRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ServiceResult<SavingGoalResponse>> GetSavingGoalAsync(int id)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var savingGoal = await _savingGoalRepository.GetAsync(id, userId!);

                if (savingGoal == null)
                {
                    return FailureResult<SavingGoalResponse>("Saving goal not found", StatusCodes.Status404NotFound);
                }

                var savingGoalResponse = _mapper.Map<SavingGoalResponse>(savingGoal);
                return SuccessResult<SavingGoalResponse>("Saving goal retrieved successfully", StatusCodes.Status200OK, savingGoalResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<SavingGoalResponse>(ex);
            }
        }

        public async Task<ServiceResult<List<SavingGoalResponse>>> GetSavingGoalsAsync()
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var savingGoals = await _savingGoalRepository.GetAllAsync(userId!);
                var savingGoalsResponse = _mapper.Map<List<SavingGoalResponse>>(savingGoals);

                return SuccessResult<List<SavingGoalResponse>>("Saving goal retrieved successfully", StatusCodes.Status200OK, savingGoalsResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<List<SavingGoalResponse>>(ex);
            }
        }

        public async Task<ServiceResult<SavingGoalResponse>> CreateSavingGoal(CreateSavingGoalRequest createSavingGoalRequest)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var savingGoal = _mapper.Map<FlowTracker.Data.Entities.SavingGoal>(createSavingGoalRequest);
                savingGoal.UserId = userId!;

                await _savingGoalRepository.AddAsync(savingGoal);
                await _savingGoalRepository.SaveAsync();

                var savingGoalResponse = _mapper.Map<SavingGoalResponse>(savingGoal);

                return SuccessResult<SavingGoalResponse>("Saving goal created successfully", StatusCodes.Status201Created, savingGoalResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<SavingGoalResponse>(ex);
            }
        }

        public async Task<ServiceResult> UpdateSavingGoal(UpdateSavingGoalRequest updateSavingGoalRequest)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var savingGoal = await _savingGoalRepository.GetAsync(updateSavingGoalRequest.Id, userId!);

                if (savingGoal == null)
                {
                    return FailureResult("Saving goal not found", StatusCodes.Status404NotFound);
                }

                _mapper.Map(updateSavingGoalRequest, savingGoal);
                await _savingGoalRepository.SaveAsync();
                return SuccessResult("Saving goal updated successfully", StatusCodes.Status204NoContent);
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

        public async Task<ServiceResult> DeleteSavingGoal(int id)
        {
            try
            {
                var userId = await GetUserIdCachedAsync();
                var savingGoal = await _savingGoalRepository.GetAsync(id, userId!);

                if (savingGoal == null)
                {
                    return FailureResult("Saving goal not found", StatusCodes.Status404NotFound);
                }

                await _savingGoalRepository.DeleteAsync(savingGoal);
                await _savingGoalRepository.SaveAsync();

                return SuccessResult("Saving goal deleted successfully", StatusCodes.Status204NoContent);

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
