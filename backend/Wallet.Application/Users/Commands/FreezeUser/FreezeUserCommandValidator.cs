using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Users.Commands.FreezeUser
{
    public class FreezeUserCommandValidator : AbstractValidator<FreezeUserCommand>
    {
        public FreezeUserCommandValidator()
        {
            RuleFor(u => u.UserId).NotEmpty().WithMessage("UserId is required.");
        }
    }
}
