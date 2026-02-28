using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Data.Repositories;
using FlowTracker.Shared.Dtos.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace FlowTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponse>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetAsync(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            var categoryResponse = _mapper.Map<CategoryResponse>(category);

            return Ok(categoryResponse);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryResponse>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoriesResponse = _mapper.Map<List<CategoryResponse>>(categories);

            return Ok(categoriesResponse);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            if (createCategoryRequest == null)
            {
                return BadRequest();
            }

            var category = _mapper.Map<Category>(createCategoryRequest);
            await _categoryRepository.AddAsync(category);
            int saveResult = await _categoryRepository.SaveAsync();

            if (!(saveResult > 0))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Valor no esperado al crear nueva categoría");
            }

            var categoryResponse = _mapper.Map<CategoryResponse>(category);

            return CreatedAtRoute("GetCategory", new { id = category.Id }, categoryResponse);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            if (updateCategoryRequest == null)
            {
                return BadRequest();
            }

            if (updateCategoryRequest.Id != id)
            {
                return BadRequest();
            }

            var category = _mapper.Map<Category>(updateCategoryRequest);
            await _categoryRepository.UpdateAsync(category);
            int saveResult = await _categoryRepository.SaveAsync();

            if (!(saveResult > 0))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Valor no esperado al actualizar categoría");
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var exits = await _categoryRepository.ExitsAsync(id);

            if (!exits)
            {
                return NotFound("Categoria no encontrada");
            }

            await _categoryRepository.DeleteAsync(id);
            int saveResult = await _categoryRepository.SaveAsync();

            if (!(saveResult > 0))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Valor no esperado al borrar categoría");
            }

            return NoContent();
        }

    }
}
