using System;
using Wallet.Application.Wallets.Commands.ActivateWallet;
using Xunit;

namespace Wallet.Tests.Application.Wallets.Commands.ActivateWallet
{
    public class ActivateWalletCommandValidatorTests
    {
        private readonly ActivateWalletCommandValidator _validator;

        public ActivateWalletCommandValidatorTests()
        {
            _validator = new ActivateWalletCommandValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenWalletIdIsValid()
        {
            // Arrange
            var command = new ActivateWalletCommand(Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_ShouldFail_WhenWalletIdIsEmpty()
        {
            // Arrange
            var command = new ActivateWalletCommand(Guid.Empty);

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "WalletId");
        }
    }
}
