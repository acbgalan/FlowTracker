using System;
using System.Collections.Generic;
using System.Text;
using FlowTracker.Shared.Dtos.SavingGoal;
using FluentValidation;

namespace FlowTracker.Shared.Validators.SavingGoal
{
    public class UpdateSavingGoalRequestValidator : AbstractValidator<UpdateSavingGoalRequest>
    {
        public UpdateSavingGoalRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The name is required.")
                .MaximumLength(100).WithMessage("The name must not exceed 100 characters.");

            RuleFor(x => x.TargetAmount)
                .GreaterThan(0).WithMessage("A valid target amount is required.");
        }
    }
}
