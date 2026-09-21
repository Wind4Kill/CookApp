using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookApp.Application.Authentication.DTOs;
using FluentValidation;

namespace CookApp.Api.Validators
{
    public class UserRegistrationValidator : AbstractValidator<UserRegisterDTO>
    {
        public UserRegistrationValidator()
        {
            RuleFor(u => u.Login).NotEmpty().WithMessage("User login can't be empty.")
            .MinimumLength(10).WithMessage("User login must be at least 10 characters in length.")
            .MaximumLength(30).WithMessage("User login must be maximum 30 characters in length.");
            RuleFor(u => u.Email).EmailAddress().WithMessage("Input user email has an inappropriate format.");
            RuleFor(u => u.Password).NotEmpty().WithMessage("User password can't be empty.")
            .MaximumLength(50).WithMessage("User password can have maximum length of 50 characters.");
        }
    }
}