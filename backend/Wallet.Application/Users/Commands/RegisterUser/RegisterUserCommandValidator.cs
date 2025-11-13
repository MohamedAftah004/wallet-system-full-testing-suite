using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {

        public RegisterUserCommandValidator()
        {

            RuleFor(x => x.FullName)
              .NotEmpty().WithMessage("Full name is required")
              .MaximumLength(100);

            RuleFor(x => x.Email)
              .NotEmpty().WithMessage("Email is required")
              .MaximumLength(50);

            RuleFor(x => x.PhoneNumber)
              .NotEmpty().WithMessage("Phone number is required")
              .Matches(@"^\+?[0-9]{10,15}$")
              .WithMessage("Phone number format is invalid");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Password is required")
               .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
        }

    }
}
