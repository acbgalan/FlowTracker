using FlowTracker.Shared.Dtos.User;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Validators.User
{
    public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("The first name is required and cannot be empty.")
                .MaximumLength(50).WithMessage("The first name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("The last name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The email is required and cannot be empty.")
                .MaximumLength(256).WithMessage("The email cannot exceed 256 characters.")
                .EmailAddress().WithMessage("The email is not in a valid format.");


            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password is required and cannot be empty.")
                .MinimumLength(8).WithMessage("The password must be at least 8 characters long.");
        }
    }
}
