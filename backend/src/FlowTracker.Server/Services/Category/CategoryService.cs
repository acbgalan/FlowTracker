using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Data.Repositories;
using FlowTracker.Server.Services.Common;
using FlowTracker.Shared.Dtos.Category;
using FlowTracker.Shared.Dtos.Common;
using Microsoft.EntityFrameworkCore;

namespace FlowTracker.Server.Services.Category
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
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
                    return SuccessResult<CategoryResponse>("Category retrieved successfully", StatusCodes.Status200OK, categoryResponse);
                }
                else
                {
                    return FailureResult<CategoryResponse>("Category not found", StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {
                return HandleGeneralException<CategoryResponse>(ex);
            }
        }

        public async Task<ServiceResult<List<CategoryResponse>>> GetCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var categoriesResponse = _mapper.Map<List<CategoryResponse>>(categories);

                return SuccessResult<List<CategoryResponse>>("Categories retrieved successfully", StatusCodes.Status200OK, categoriesResponse);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<List<CategoryResponse>>(ex);
            }
        }

        public async Task<ServiceResult<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest)
        {
            try
            {
                if (createCategoryRequest == null)
                {
                    return FailureResult<CategoryResponse>("The request object is null", StatusCodes.Status400BadRequest);
                }

                bool exitsNameAndType = await _categoryRepository.ExitsByNameAndTypeAsync(createCategoryRequest.Name, createCategoryRequest.Type);
                if (exitsNameAndType)
                {
                    return FailureResult<CategoryResponse>("A category with that name and type already exists.", StatusCodes.Status409Conflict);
                }

                var category = _mapper.Map<Data.Entities.Category>(createCategoryRequest);
                var userId = await _currentUserService.GetUserIdAsync();
                category.UserId = userId;
                await _categoryRepository.AddAsync(category);
                int saveResult = await _categoryRepository.SaveAsync();

                if (saveResult > 0)
                {
                    var categoryResponse = _mapper.Map<CategoryResponse>(category);
                    return SuccessResult<CategoryResponse>("Category created successfully", StatusCodes.Status201Created, categoryResponse);
                }
                else
                {
                    return FailureResult<CategoryResponse>("Unexpected value when creating a category", StatusCodes.Status500InternalServerError);
                }
            }
            catch (DbUpdateException ex)
            {
                return HandleGeneralException<CategoryResponse>(ex);
            }
            catch (Exception ex)
            {
                return HandleGeneralException<CategoryResponse>(ex);
            }
        }

        public async Task<ServiceResult> UpdateCategoryAsync(UpdateCategoryRequest updateCategoryRequest)
        {
            try
            {
                if (updateCategoryRequest == null)
                {
                    return FailureResult("The request object is null", StatusCodes.Status400BadRequest);
                }

                var userId = await _currentUserService.GetUserIdAsync();
                var category = await _categoryRepository.GetAsync(updateCategoryRequest.Id, userId!);

                if (category == null)
                {
                    return FailureResult("User is not allowed to update the category", StatusCodes.Status403Forbidden);
                }

                _mapper.Map(updateCategoryRequest, category);
                await _categoryRepository.UpdateAsync(category);
                int saveResult = await _categoryRepository.SaveAsync();

                if (saveResult > 0)
                {
                    return SuccessResult("Category updated successfully", StatusCodes.Status204NoContent);
                }
                else
                {
                    return FailureResult("Unexpected value when updating a category", StatusCodes.Status500InternalServerError);
                }
            }
            catch (DbUpdateException ex)
            {
                return HandleDbUpdateException(ex);
            }
            catch (Exception ex)
            {
                return HandleGeneralException(ex);
            }
        }

        public async Task<ServiceResult> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetAsync(id);

                if (category == null)
                {
                    return FailureResult("Category not found", StatusCodes.Status404NotFound);
                }

                var userId = await _currentUserService.GetUserIdAsync();

                if (category.UserId != userId)
                {
                    return FailureResult("User is not allowed to delete the category", StatusCodes.Status403Forbidden);
                }

                await _categoryRepository.DeleteAsync(category);

                int saveResult = await _categoryRepository.SaveAsync();

                if (saveResult > 0)
                {
                    return SuccessResult("Category deleted successfully", StatusCodes.Status204NoContent);
                }
                else
                {
                    return FailureResult("Unexpected value when deleting category", StatusCodes.Status500InternalServerError);
                }
            }
            catch (DbUpdateException ex)
            {
                return HandleDbUpdateException(ex);
            }
            catch (Exception ex)
            {
                return HandleGeneralException(ex);
            }
        }

        public async Task<bool> IsCategoryValidForUserAsync(int id)
        {
            var userId = await _currentUserService.GetUserIdAsync();
            return await _categoryRepository.IsCategoryValidForUserAsync(id, userId!);
        }

        public async Task<bool> IsCategoryValidForUserAsync(int id, string userId)
        {
            return await _categoryRepository.IsCategoryValidForUserAsync(id, userId);
        }
    }
}