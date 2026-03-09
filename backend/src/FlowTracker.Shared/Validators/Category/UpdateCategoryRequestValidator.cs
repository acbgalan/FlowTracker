using FlowTracker.Shared.Dtos.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FlowTracker.Shared.Validators.Category
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The name is required and cannot be empty.")
                .Length(2, 50).WithMessage("The name must be between 2 and 50 characters long.");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("The transaction type provided is not valid.");

            RuleFor(x => x.Icon)
                .MaximumLength(100).WithMessage("The icon cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotNull().WithMessage("The description cannot be null.")
                .MaximumLength(250).WithMessage("The description must not exceed 250 characters.");
        }
    }
}
