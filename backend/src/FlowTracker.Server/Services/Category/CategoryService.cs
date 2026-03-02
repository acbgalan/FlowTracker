using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Data.Repositories;
using FlowTracker.Shared.Dtos.Category;
using FlowTracker.Shared.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace FlowTracker.Server.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult<CategoryResponse>> GetCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(id);
                var categoryResponse = category != null ? _mapper.Map<CategoryResponse>(category) : null;

                if (category != null)
                {
                    return new ServiceResult<CategoryResponse>
                    {
                        Success = true,
                        Message = "Category retrieved successfully",
                        StatusCode = StatusCodes.Status200OK,
                        Data = categoryResponse
                    };
                }
                else
                {
                    return new ServiceResult<CategoryResponse>
                    {
                        Success = false,
                        Message = "Category not found",
                        StatusCode = StatusCodes.Status404NotFound,
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new ServiceResult<CategoryResponse>
                {
                    Success = false,
                    Message = $"Unexpected error: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null
                };
            }
        }

        public async Task<ServiceResult<List<CategoryResponse>>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var categoriesResponse = _mapper.Map<List<CategoryResponse>>(categories);

                return new ServiceResult<List<CategoryResponse>>
                {
                    Success = true,
                    Message = "Categories retrieved successfully",
                    StatusCode = StatusCodes.Status200OK,
                    Data = categoriesResponse
                };

            }
            catch (Exception ex)
            {
                return new ServiceResult<List<CategoryResponse>>
                {
                    Success = false,
                    Message = $"Unexpected error: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null
                };
            }
        }

        public async Task<ServiceResult<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest)
        {
            try
            {
                if (createCategoryRequest == null)
                {
                    return new ServiceResult<CategoryResponse>
                    {
                        Success = false,
                        Message = "The request object is null",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Data = null
                    };
                }

                var category = _mapper.Map<Data.Entities.Category>(createCategoryRequest);
                await _categoryRepository.AddAsync(category);
                int saveResult = await _categoryRepository.SaveAsync();

                if (saveResult > 0)
                {
                    var categoryResponse = _mapper.Map<CategoryResponse>(category);
                    return new ServiceResult<CategoryResponse>
                    {
                        Success = true,
                        Message = "Category created successfully",
                        StatusCode = StatusCodes.Status201Created,
                        Data = categoryResponse
                    };
                }
                else
                {
                    return new ServiceResult<CategoryResponse>
                    {
                        Success = false,
                        Message = "Unexpected value when creating a category",
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Data = null
                    };
                }

            }
            catch (DbUpdateException ex)
            {
                return new ServiceResult<CategoryResponse>
                {
                    Success = false,
                    Message = $"Database error: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<CategoryResponse>
                {
                    Success = false,
                    Message = $"Unexpected error: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Data = null
                };

            }
        }

        public async Task<ServiceResult> UpdateCategoryAsync(UpdateCategoryRequest updateCategoryRequest)
        {
            try
            {
                if (updateCategoryRequest == null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "The request object is null",
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                var category = _mapper.Map<Data.Entities.Category>(updateCategoryRequest);
                await _categoryRepository.UpdateAsync(category);
                int saveResult = await _categoryRepository.SaveAsync();

                if (saveResult > 0)
                {
                    return new ServiceResult
                    {
                        Success = true,
                        Message = "Category updated successfully",
                        StatusCode = StatusCodes.Status204NoContent
                    };
                }
                else
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Unexpected value when updating a category",
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (DbUpdateException ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Database error: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Unexpected error: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        public async Task<ServiceResult> DeleteCategoryAsync(int id)
        {
            try
            {
                var exits = await _categoryRepository.ExitsAsync(id);

                if (!exits)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Category not found",
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                await _categoryRepository.DeleteAsync(id);
                int saveResult = await _categoryRepository.SaveAsync();

                if (saveResult > 0)
                {
                    return new ServiceResult
                    {
                        Success = true,
                        Message = "Category deleted successfully",
                        StatusCode = StatusCodes.Status204NoContent
                    };
                }
                else
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Unexpected value when deleting category",
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (DbUpdateException ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Database error: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Unexpected error: {ex.Message}",
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        //TODO: SuccessResult, FailureResult

    }
}
