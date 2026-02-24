using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Category;

namespace FlowTracker.Server.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category, CategoryResponse>();
        }
    }
}
