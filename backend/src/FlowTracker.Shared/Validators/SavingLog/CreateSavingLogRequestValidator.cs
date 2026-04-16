using FlowTracker.Shared.Dtos.SavingLog;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Validators.SavingLog
{
    public class CreateSavingLogRequestValidator : AbstractValidator<CreateSavingLogRequest>
    {
        public CreateSavingLogRequestValidator()
        {
            RuleFor(x => x.SavingGoalId)
                .GreaterThan(0).WithMessage("A valid SavingGoalId is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("A valid movement amount amount is required.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("The movement type provided is not valid.");
        }
    }
}
