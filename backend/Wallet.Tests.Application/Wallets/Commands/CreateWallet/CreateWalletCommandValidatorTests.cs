using System;
using Wallet.Application.Wallets.Commands.CreateWallet;
using Xunit;

namespace Wallet.Tests.Application.Wallets.Commands.CreateWallet
{
    public class CreateWalletCommandValidatorTests
    {
        private readonly CreateWalletCommandValidator _validator;

        public CreateWalletCommandValidatorTests()
        {
            _validator = new CreateWalletCommandValidator();
        }

        [Fact]
        public void Validate_ShouldPass_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateWalletCommand(Guid.NewGuid(), "EGP");

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validate_ShouldFail_WhenUserIdIsEmpty()
        {
            // Arrange
            var command = new CreateWalletCommand(Guid.Empty, "EGP");

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "UserId");
        }

        [Fact]
        public void Validate_ShouldFail_WhenCurrencyCodeIsEmpty()
        {
            // Arrange
            var command = new CreateWalletCommand(Guid.NewGuid(), "");

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "CurrencyCode");
        }

        [Fact]
        public void Validate_ShouldFail_WhenCurrencyCodeIsInvalid()
        {
            // Arrange
            var command = new CreateWalletCommand(Guid.NewGuid(), "12$");

            // Act
            var result = _validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "CurrencyCode");
        }
    }
}
