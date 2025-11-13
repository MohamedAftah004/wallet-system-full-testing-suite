using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Commands.DisableUser
{
    public class DisableUserCommandValidator : AbstractValidator<DisableUserCommand>
    {

        public DisableUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

        }

    }
}
