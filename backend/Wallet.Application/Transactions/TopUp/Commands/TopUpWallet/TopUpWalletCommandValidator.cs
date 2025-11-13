using FluentValidation;

namespace Wallet.Application.Transactions.TopUp.Commands.TopUpWallet
{
    public class TopUpWalletCommandValidator : AbstractValidator<TopUpWalletCommand>
    {
        public TopUpWalletCommandValidator()
        {
            RuleFor(x => x.WalletId)
                .NotEmpty().WithMessage("Wallet ID is required.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

           
        }
    }
}
