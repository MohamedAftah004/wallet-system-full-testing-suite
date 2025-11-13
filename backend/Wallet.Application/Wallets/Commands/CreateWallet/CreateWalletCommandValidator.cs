using FluentValidation;

namespace Wallet.Application.Wallets.Commands.CreateWallet
{
    public class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
    {
        public CreateWalletCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required");

            RuleFor(x => x.CurrencyCode)
                .NotEmpty().WithMessage("Currency is required")
                .Length(3).WithMessage("Currency code length must be 3 letters")
                .Matches("^[A-Za-z]{3}$").WithMessage("Currency code must contain only letters (e.g. USD, EGP).");
        }
    }
}
