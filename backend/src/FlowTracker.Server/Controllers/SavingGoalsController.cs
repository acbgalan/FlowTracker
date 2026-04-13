using FlowTracker.Server.Services.SavingGoal;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.SavingGoal;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingGoalsController : ControllerBase
    {
        private readonly ISavingGoalService _savingGoalService;
        private readonly IValidator<CreateSavingGoalRequest> _createSavingGoalRequestValidator;

        public SavingGoalsController(ISavingGoalService savingGoalService, IValidator<CreateSavingGoalRequest> createSavingGoalRequestValidator)
        {
            _savingGoalService = savingGoalService;
            _createSavingGoalRequestValidator = createSavingGoalRequestValidator;
        }

        [HttpGet("{id:int}", Name = "GetSavingGoal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SavingGoalResponse>> GetSavingGoal(int id)
        {
            var serviceResult = await _savingGoalService.GetSavingGoalAsync(id);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return Ok(serviceResult.Data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SavingGoalResponse>>> GetAllSavingGoals()
        {
            var serviceResult = await _savingGoalService.GetSavingGoalsAsync();

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return Ok(serviceResult.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceResult<SavingGoalResponse>>> CreateSavingGoal(CreateSavingGoalRequest createSavingGoalRequest)
        {
            // 1. DTO rule validation
            var validationResult = _createSavingGoalRequestValidator.Validate(createSavingGoalRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }


            // 2. Service call
            var serviceResult = await _savingGoalService.CreateSavingGoal(createSavingGoalRequest);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return CreatedAtRoute("GetSavingGoal", new { id = serviceResult.Data!.Id }, serviceResult.Data);
        }

    }
}
