using System;
using Wallet.Application.Wallets.Commands.CloseWallet;
using Xunit;

namespace Wallet.Tests.Application.Wallets.Commands.CloseWallet
{
    public class CloseWalletCommandValidatorTests
    {
        private readonly CloseWalletCommandValidator _validator;

        public CloseWalletCommandValidatorTests()
        {
            _validator = new CloseWalletCommandValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenWalletIdIsValid()
        {
            // Arrange
            var command = new CloseWalletCommand(Guid.NewGuid());

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_ShouldFail_WhenWalletIdIsEmpty()
        {
            // Arrange
            var command = new CloseWalletCommand(Guid.Empty);

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "WalletId");
        }
    }
}
