using FlowTracker.Server.Services.SavingGoal;
using FlowTracker.Shared.Dtos.SavingGoal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingGoalsController : ControllerBase
    {
        private readonly ISavingGoalService _savingGoalService;

        public SavingGoalsController(ISavingGoalService savingGoalService)
        {
            _savingGoalService = savingGoalService;
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
        public async Task<ActionResult<List<SavingGoalResponse>>> GetAllSavingGoals()
        {
            var serviceResult = await _savingGoalService.GetSavingGoalsAsync();

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return Ok(serviceResult.Data);
        }

    }
}
