using AutoMapper;
using FlowTracker.Data.Entities;
using FlowTracker.Shared.Dtos.Category;
using FlowTracker.Shared.Dtos.SavingGoal;
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
            TransactionMappings();
            SavingGoalMappings();
        }

        private void CategoryMappings()
        {
            CreateMap<Category, CategoryResponse>();
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>()
                .ForMember(d => d.Icon, o => o.Condition((src, dest, srcMember) => srcMember != null));
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
            CreateMap<UpdateTransactionRequest, Transaction>();
        }

        private void SavingGoalMappings()
        {
            CreateMap<SavingGoal, SavingGoalResponse>()
                .ForMember(d => d.CurrentAmount, o => o.MapFrom(s => s.Transactions.Sum(x => x.Amount)))
                .ForMember(d => d.ProgressPercentaje, o => o.MapFrom(s => s.TargetAmount > 0 ? Math.Round((s.Transactions.Sum(x => x.Amount) / s.TargetAmount) * 100, 2) : 0));

            CreateMap<CreateSavingGoalRequest, SavingGoal>();
            CreateMap<UpdateSavingGoalRequest, SavingGoal>();
        }

    }
}
