using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Users.Commands.ActivateUser;

namespace Wallet.Application.Users.Commands.CloseUser
{

    public class CloseUserCommandValidator : AbstractValidator<CloseUserCommand>
    {

        public CloseUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }

    }
}
