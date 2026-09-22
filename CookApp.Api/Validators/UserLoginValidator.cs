using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Application.Authentication;
using FluentValidation;

namespace CookApp.Api.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.Email).NotEmpty().WithMessage("Email field can't be empty.")
            .EmailAddress().WithMessage("Input user email has an inappropriate format.");
            RuleFor(u => u.Password).NotEmpty().WithMessage("User password can't be empty.")
            .MaximumLength(50).WithMessage("User password can have maximum length of 50 characters.");
        }
    }
}