using FlowTracker.Server.Services.SavingLog;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Dtos.SavingLog;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SavingLogsController : ControllerBase
    {
        private readonly ISavingLogService _savingLogService;
        private readonly IValidator<CreateSavingLogRequest> _createSavingLogRequestValidator;

        public SavingLogsController(ISavingLogService savingLogService, IValidator<CreateSavingLogRequest> createSavingLogRequestValidator)
        {
            _savingLogService = savingLogService;
            _createSavingLogRequestValidator = createSavingLogRequestValidator;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SavingLogResponse>> CreateSavingLog([FromBody] CreateSavingLogRequest createSavingLogRequest)
        {
            // 1. DTO validation
            var validationResult = _createSavingLogRequestValidator.Validate(createSavingLogRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            // 2. Service call
            var serviceResult = await _savingLogService.CreateSavingLogAsync(createSavingLogRequest);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return CreatedAtRoute("GetSavingLog", new { id = serviceResult.Data!.Id }, serviceResult.Data);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateSavingLog(int id, [FromBody] UpdateSavingLogRequest updateSavingLogRequest)
        {
            // 1. DTO validation
            if (id != updateSavingLogRequest.Id)
            {
                return BadRequest("Id mismatch");
            }

            // 2. Service call
            var serviceResult = await _savingLogService.UpdateSavingLogAsync(updateSavingLogRequest);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteSavingLog(int id)
        {
            var serviceResult = await _savingLogService.DeleteSavingLogAsync(id);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return NoContent();
        }


    }
}
