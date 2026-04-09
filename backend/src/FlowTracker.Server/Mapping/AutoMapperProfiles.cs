using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Category;
using FlowTracker.Shared.Dtos.Transaction;
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

        private void TransactionMappings()
        {
            CreateMap<Transaction, TransactionResponse>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Category.Type))
                .ForMember(d => d.Icon, o => o.MapFrom(s => s.Category.Icon))
                .ForMember(d => d.CategoryDescription, o => o.MapFrom(s => s.Category.Description));

            CreateMap<CreateTransactionRequest, Transaction>();
        }

    }
}
