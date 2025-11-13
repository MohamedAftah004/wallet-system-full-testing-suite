using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Wallets.Commands.ActivateWallet
{
    public class ActivateWalletCommandValidator : AbstractValidator<ActivateWalletCommand>
    {

        public ActivateWalletCommandValidator()
        {
            RuleFor(x => x.WalletId)
                .NotEmpty().WithMessage("Wallet ID is required.")
                .Must(id => id != Guid.Empty).WithMessage("Wallet ID cannot be an empty GUID.");
        }

    }
}
