using FlowTracker.Server.Services.SavingLog;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.SavingLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingLogsController : ControllerBase
    {
        private readonly ISavingLogService _savingLogService;

        public SavingLogsController(ISavingLogService savingLogService)
        {
            _savingLogService = savingLogService;
        }

        [HttpGet("{id:int}", Name = "GetSavingLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SavingLogResponse>> GetSavingLog(int id)
        {
            var serviceResult = await _savingLogService.GetSavingLogAsync(id);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return Ok(serviceResult.Data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<SavingLogResponse>>> GetAllSavingLogs()
        {
            var serviceResult = await _savingLogService.GetSavingLogsAsync();

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return Ok(serviceResult.Data);
        }



    }
}
