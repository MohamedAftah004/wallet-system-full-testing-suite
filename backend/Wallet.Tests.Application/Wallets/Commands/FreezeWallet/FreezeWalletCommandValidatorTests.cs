using System;
using FluentValidation.TestHelper;
using Wallet.Application.Wallets.Commands.FreezeWallet;
using Xunit;

namespace Wallet.Tests.Application.Wallets.Commands.FreezeWallet
{
    public class FreezeWalletCommandValidatorTests
    {
        private readonly FreezeWalletCommandValidator _validator;

        public FreezeWalletCommandValidatorTests()
        {
            _validator = new FreezeWalletCommandValidator();
        }

        [Fact]
        public void Should_HaveError_WhenWalletIdIsEmpty()
        {
            // Arrange
            var command = new FreezeWalletCommand(Guid.Empty);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.WalletId)
                  .WithErrorMessage("Wallet ID is required.");
        }

        [Fact]
        public void Should_PassValidation_WhenWalletIdIsValid()
        {
            // Arrange
            var command = new FreezeWalletCommand(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
