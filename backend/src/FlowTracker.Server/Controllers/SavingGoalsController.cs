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
        private readonly IValidator<UpdateSavingGoalRequest> _updateSavingGoalRequestValidator;

        public SavingGoalsController(
            ISavingGoalService savingGoalService,
            IValidator<CreateSavingGoalRequest> createSavingGoalRequestValidator,
            IValidator<UpdateSavingGoalRequest> updateSavingGoalRequestValidator)
        {
            _savingGoalService = savingGoalService;
            _createSavingGoalRequestValidator = createSavingGoalRequestValidator;
            _updateSavingGoalRequestValidator = updateSavingGoalRequestValidator;
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

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateSavingGoal(int id, [FromBody] UpdateSavingGoalRequest updateSavingGoalRequest)
        {
            // 1. Fast validation
            if (id != updateSavingGoalRequest.Id)
            {
                return BadRequest("Id mismatch");
            }

            var validationResult = _updateSavingGoalRequestValidator.Validate(updateSavingGoalRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            // 2. Service call
            var serviceResult = await _savingGoalService.UpdateSavingGoal(updateSavingGoalRequest);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteSavingGoal(int id)
        {
            var serviceResult = await _savingGoalService.DeleteSavingGoal(id);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return NoContent();
        }


    }
}
