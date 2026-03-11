using FlowTracker.Shared.Dtos.User;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Shared.Validators.User
{
    public class UserCredentialsRequestValidator : AbstractValidator<UserCredentialsRequest>
    {
        public UserCredentialsRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("The email is required and cannot be empty.")
                .MaximumLength(256).WithMessage("The email cannot exceed 256 characters.")
                .EmailAddress().WithMessage("The email is not in a valid format");


            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("The password is required and cannot be empty.")
                .MinimumLength(10).WithMessage("The password must be at least 10 characters long.");

        }
    }
}
