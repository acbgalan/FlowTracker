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
        private string? _userId;

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
                var userId = await GetUserIdCachedAsync();
                var category = await _categoryRepository.GetAsync(id, userId!);
                var categoryResponse = _mapper.Map<CategoryResponse>(category);

                if (category == null)
                {
                    return FailureResult<CategoryResponse>("Category not found", StatusCodes.Status404NotFound);
                }

                return SuccessResult<CategoryResponse>("Category retrieved successfully", StatusCodes.Status200OK, categoryResponse);
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
                var userId = await GetUserIdCachedAsync();
                var categories = await _categoryRepository.GetAllAsync(userId!);
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

                var userId = await GetUserIdCachedAsync();
                bool exitsNameAndType = await _categoryRepository.ExitsByNameAndTypeAsync(createCategoryRequest.Name, createCategoryRequest.Type, userId!);

                if (exitsNameAndType)
                {
                    return FailureResult<CategoryResponse>("A category with that name and type already exists.", StatusCodes.Status409Conflict);
                }

                var category = _mapper.Map<Data.Entities.Category>(createCategoryRequest);
                category.UserId = userId;
                await _categoryRepository.AddAsync(category);
                await _categoryRepository.SaveAsync();

                var categoryResponse = _mapper.Map<CategoryResponse>(category);
                return SuccessResult<CategoryResponse>("Category created successfully", StatusCodes.Status201Created, categoryResponse);
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

                var userId = await GetUserIdCachedAsync();
                var category = await _categoryRepository.GetAsync(updateCategoryRequest.Id, userId!);

                if (category == null)
                {
                    return FailureResult("User is not allowed to update the category", StatusCodes.Status403Forbidden);
                }

                _mapper.Map(updateCategoryRequest, category);
                await _categoryRepository.UpdateAsync(category);
                await _categoryRepository.SaveAsync();

                return SuccessResult("Category updated successfully", StatusCodes.Status204NoContent);
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
                var userId = await GetUserIdCachedAsync();
                var category = await _categoryRepository.GetAsync(id, userId!);

                if (category == null)
                {
                    return FailureResult("Category not found", StatusCodes.Status404NotFound);
                }

                await _categoryRepository.DeleteAsync(category);
                await _categoryRepository.SaveAsync();

                return SuccessResult("Category deleted successfully", StatusCodes.Status204NoContent);
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
            var userId = await GetUserIdCachedAsync();
            return await _categoryRepository.IsCategoryValidForUserAsync(id, userId!);
        }

        public async Task<bool> IsCategoryValidForUserAsync(int id, string userId)
        {
            return await _categoryRepository.IsCategoryValidForUserAsync(id, userId);
        }

        private async Task<string?> GetUserIdCachedAsync()
        {
            if (_userId == null)
            {
                _userId = await _currentUserService.GetUserIdAsync();
            }

            return _userId;
        }

    }
}