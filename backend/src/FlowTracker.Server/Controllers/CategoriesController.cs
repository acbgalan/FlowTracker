using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Data.Repositories;
using FlowTracker.Server.Services.Category;
using FlowTracker.Shared.Dtos.Category;
using FlowTracker.Shared.Dtos.Common;
using FlowTracker.Shared.Validators.Category;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace FlowTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<CreateCategoryRequest> _createCategoryRequestValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateCategoryRequestValidator;

        public CategoriesController(
            ICategoryService categoryService,
            IValidator<CreateCategoryRequest> createCategoryRequestValidator,
            IValidator<UpdateCategoryRequest> updateCategoryRequestValidator)
        {
            _categoryService = categoryService;
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _updateCategoryRequestValidator = updateCategoryRequestValidator;
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> GetCategory(int id)
        {
            var serviceResult = await _categoryService.GetCategoryAsync(id);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return Ok(serviceResult.Data);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<CategoryResponse>>> GetAllCategories([FromQuery] QueryParameters queryParameters)
        {
            var serviceResult = await _categoryService.GetCategoriesAsync(queryParameters);

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
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            // 1. DTO rule validation (length, format, etc.)
            var validationResult = _createCategoryRequestValidator.Validate(createCategoryRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            // 2. Service call (business logic)
            var serviceResult = await _categoryService.CreateCategoryAsync(createCategoryRequest);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return CreatedAtRoute("GetCategory", new { id = serviceResult.Data!.Id }, serviceResult.Data);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            // 1. Fast validations
            if (id != updateCategoryRequest.Id)
            {
                return BadRequest("Id mismatch");
            }

            var validationResult = _updateCategoryRequestValidator.Validate(updateCategoryRequest);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToDictionary());
            }

            // 2. Service call (business logic)
            var serviceResult = await _categoryService.UpdateCategoryAsync(updateCategoryRequest);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var serviceResult = await _categoryService.DeleteCategoryAsync(id);

            if (!serviceResult.Success)
            {
                return StatusCode(serviceResult.StatusCode, serviceResult.Message);
            }

            return NoContent();
        }


    }
}
