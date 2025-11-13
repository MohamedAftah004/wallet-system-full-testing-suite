using System;
using FluentValidation.TestHelper;
using Wallet.Application.Wallets.Queries.GetUserWallets;
using Xunit;

namespace Wallet.Tests.Application.Wallets.Queries.GetUserWallets
{
    public class GetUserWalletsQueryValidatorTests
    {
        private readonly GetUserWalletsQueryValidator _validator;

        public GetUserWalletsQueryValidatorTests()
        {
            _validator = new GetUserWalletsQueryValidator();
        }

        [Fact]
        public void Should_HaveError_WhenUserIdIsEmpty()
        {
            // Arrange
            var query = new GetUserWalletsQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserId)
                .WithErrorMessage("User ID is required.");
        }

        [Fact]
        public void Should_PassValidation_WhenUserIdIsValid()
        {
            // Arrange
            var query = new GetUserWalletsQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
