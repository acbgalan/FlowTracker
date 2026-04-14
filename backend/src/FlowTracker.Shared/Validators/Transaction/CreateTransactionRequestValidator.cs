using FlowTracker.Shared.Dtos.Transaction;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Validators.Transaction
{
    public class CreateTransactionRequestValidator : AbstractValidator<CreateTransactionRequest>
    {
        public CreateTransactionRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0).WithMessage("The amount must be greater than 0.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("The date is required.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("The description must not exceed 250 characters.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("A valid category is required.");
        }
    }
}
