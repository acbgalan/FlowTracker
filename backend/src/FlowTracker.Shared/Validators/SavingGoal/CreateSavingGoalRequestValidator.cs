using FlowTracker.Shared.Dtos.SavingGoal;
using FluentValidation;

namespace FlowTracker.Shared.Validators.SavingGoal
{
    public class CreateSavingGoalRequestValidator : AbstractValidator<CreateSavingGoalRequest>
    {
        public CreateSavingGoalRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The name is required.")
                .MaximumLength(100).WithMessage("The name must not exceed 100 characters.");

            RuleFor(x => x.TargetAmount)
                .GreaterThan(0).WithMessage("A valid target amount is required.");
        }
    }
}
