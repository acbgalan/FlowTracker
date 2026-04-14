using FlowTracker.Server.Services.Transaction;
using FlowTracker.Shared.Dtos.Transaction;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IValidator<CreateTransactionRequest> _createTransactionRequestValidator;
        private readonly IValidator<UpdateTransactionRequest> _updateTransactionRequestValidator;

        public TransactionsController(
            ITransactionService transactionService,
            IValidator<CreateTransactionRequest> createTransactionRequestValidator,
            IValidator<UpdateTransactionRequest> updateTransactionRequestValidator)
        {
            _transactionService = transactionService;
            _createTransactionRequestValidator = createTransactionRequestValidator;
            _updateTransactionRequestValidator = updateTransactionRequestValidator;
        }

        [HttpGet("{id:int}", Name = "GetTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransactionResponse>> GetTransaction(int id)
        {
            var serviceResult = await _transactionService.GetTransactionAsync(id);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return Ok(serviceResult.Data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TransactionResponse>>> GetAllTransactions()
        {
            var serviceResult = await _transactionService.GetTransactionsAsync();

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return Ok(serviceResult.Data);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransactionResponse>> CreateTransaction([FromBody] CreateTransactionRequest createTransactionRequest)
        {
            // 1. DTO rule validation
            var validationResult = _createTransactionRequestValidator.Validate(createTransactionRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            // 2. Service call
            var serviceResult = await _transactionService.CreateTransactionAsync(createTransactionRequest);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return CreatedAtRoute("GetTransaction", new { id = serviceResult.Data!.Id }, serviceResult.Data);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateTransaction(int id, [FromBody] UpdateTransactionRequest updateTransactionRequest)
        {
            // 1. Fast validations
            if (id != updateTransactionRequest.Id)
            {
                return BadRequest("Id mismatch");
            }

            var validationResult = _updateTransactionRequestValidator.Validate(updateTransactionRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            // 2. Service call (business logic)
            var serviceResult = await _transactionService.UpdateTransactionAsync(updateTransactionRequest);

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
        public async Task<ActionResult> DeleteTransaction(int id)
        {
            var serviceResult = await _transactionService.DeleteTransactionAsync(id);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return NoContent();
        }


    }
}
