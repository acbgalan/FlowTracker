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
        public async Task<ActionResult<SavingGoalResponse>> GetSavingGoal(int id)
        {
            var serviceResult = await _savingGoalService.GetSavingGoalAsync(id);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return Ok(serviceResult.Data);
        }

    }
}
