using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Data.Repositories;
using FlowTracker.Server.Services.Common;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.SavingGoal;
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
                    return FailureResult<SavingGoalResponse>("Saving Goal not found", StatusCodes.Status404NotFound);
                }

                var savingGoalResponse = _mapper.Map<SavingGoalResponse>(savingGoal);
                return SuccessResult<SavingGoalResponse>("Saving Goal retrieved successfully", StatusCodes.Status200OK, savingGoalResponse);
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

                return SuccessResult<List<SavingGoalResponse>>("", StatusCodes.Status200OK, savingGoalsResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<List<SavingGoalResponse>>(ex);
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
