using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Category;
using FlowTracker.Shared.Dtos.Common;

namespace FlowTracker.Server.Services.Category
{
    public interface ICategoryService
    {
        Task<ServiceResult<CategoryResponse>> GetCategoryAsync(int id);
        Task<ServiceResult<PagedResponse<CategoryResponse>>> GetCategoriesAsync(QueryParameters queryParameters);
        Task<ServiceResult<CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest createCategoryRequest);
        Task<ServiceResult> UpdateCategoryAsync(UpdateCategoryRequest updateCategoryRequest);
        Task<ServiceResult> DeleteCategoryAsync(int id);
        Task<bool> IsCategoryValidForUserAsync(int id);
        Task<bool> IsCategoryValidForUserAsync(int id, string userId);
    }
}
