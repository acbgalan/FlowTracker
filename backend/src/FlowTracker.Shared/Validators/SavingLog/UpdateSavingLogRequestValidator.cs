using FlowTracker.Shared.Dtos.SavingLog;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Validators.SavingLog
{
    public class UpdateSavingLogRequestValidator : AbstractValidator<UpdateSavingLogRequest>
    {
        public UpdateSavingLogRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid id is required.");

            RuleFor(x => x.SavingGoalId)
                .GreaterThan(0).WithMessage("A valid SavinGoalId is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("A valid target amount is required.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("The movement type provided is not valid.");
        }
    }
}
