using FluentValidation.TestHelper;
using System;
using Wallet.Application.Transactions.TopUp.Commands.TopUpWallet;
using Xunit;

namespace Wallet.Tests.Application.Transactions.TopUp.Commands
{
    public class TopUpWalletCommandValidatorTests
    {
        private readonly TopUpWalletCommandValidator _validator;

        public TopUpWalletCommandValidatorTests()
        {
            _validator = new TopUpWalletCommandValidator();
        }

        [Fact]
        public void Should_HaveError_WhenWalletIdIsEmpty()
        {
            var command = new TopUpWalletCommand(Guid.Empty, 100, "Test top-up");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.WalletId)
                .WithErrorMessage("Wallet ID is required.");
        }

        [Fact]
        public void Should_HaveError_WhenAmountIsZeroOrNegative()
        {
            var command = new TopUpWalletCommand(Guid.NewGuid(), 0, "Invalid amount");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Amount)
                .WithErrorMessage("Amount must be greater than zero.");
        }

        [Fact]
        public void Should_PassValidation_WhenAllFieldsValid()
        {
            var command = new TopUpWalletCommand(Guid.NewGuid(), 100, "Valid top-up");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
