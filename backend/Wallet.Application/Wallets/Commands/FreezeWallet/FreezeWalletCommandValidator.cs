using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Wallets.Commands.FreezeWallet
{
    public class FreezeWalletCommandValidator : AbstractValidator<FreezeWalletCommand>
    {
        public FreezeWalletCommandValidator()
        {
            RuleFor(x => x.WalletId)
                .NotEmpty()
                .WithMessage("Wallet ID is required.");
        }
    }
}
