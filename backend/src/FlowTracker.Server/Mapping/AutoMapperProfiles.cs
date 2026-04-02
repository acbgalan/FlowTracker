using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Category;
using FlowTracker.Shared.Dtos.User;

namespace FlowTracker.Server.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CategoryMappings();
            UserMappings();
        }

        private void CategoryMappings()
        {
            CreateMap<Category, CategoryResponse>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
        }

        private void UserMappings()
        {
            CreateMap<UserRegisterRequest, User>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email));
        }
    }
}
