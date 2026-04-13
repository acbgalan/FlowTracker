using FlowTracker.Shared.Dtos.SavingGoal;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Validators.SavingGoal
{
    public class CreateTransactionRequestValidator : AbstractValidator<CreateSavingGoalRequest>
    {
        public CreateTransactionRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The name is required.")
                .MaximumLength(100).WithMessage("The description must not exceed 100 characters.");

            RuleFor(x => x.TargetAmount)
                .GreaterThan(0).WithMessage("A valid target amount is required.");

            RuleFor(x => x.Deadline)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .When(x => x.Deadline.HasValue)
                .WithMessage("The deadline must be later than today.");
        }
    }
}
