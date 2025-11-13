using System;
using FluentValidation.TestHelper;
using Wallet.Application.Wallets.Queries.GetWalletInfoById;
using Xunit;

namespace Wallet.Tests.Application.Wallets.Queries.GetWalletInfoById
{
    public class GetWalletInfoByIdQueryValidatorTests
    {
        private readonly GetWalletInfoByIdQueryValidator _validator;

        public GetWalletInfoByIdQueryValidatorTests()
        {
            _validator = new GetWalletInfoByIdQueryValidator();
        }

        [Fact]
        public void Should_HaveError_WhenWalletIdIsEmpty()
        {
            // Arrange
            var query = new GetWalletInfoByIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.WalletId)
                .WithErrorMessage("WalletId is required");
        }

        [Fact]
        public void Should_PassValidation_WhenWalletIdIsValid()
        {
            // Arrange
            var query = new GetWalletInfoByIdQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
