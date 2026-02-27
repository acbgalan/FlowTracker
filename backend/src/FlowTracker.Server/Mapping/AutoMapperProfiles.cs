using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Category;

namespace FlowTracker.Server.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CategoryMappings();
        }

        private void CategoryMappings()
        {
            CreateMap<Category, CategoryResponse>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
        }
    }
}
