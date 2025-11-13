using FluentValidation.TestHelper;
using System;
using Wallet.Application.Transactions.History.Queries.GetTransactions;
using Xunit;

namespace Wallet.Tests.Application.Transactions.History.Queries.GetTransactions
{
    public class GetTransactionsQueryValidatorTests
    {
        private readonly GetTransactionsQueryValidator _validator;

        public GetTransactionsQueryValidatorTests()
        {
            _validator = new GetTransactionsQueryValidator();
        }

        [Fact]
        public void Should_HaveError_WhenWalletIdIsEmpty()
        {
            // Arrange
            var query = new GetTransactionsQuery(Guid.Empty, null, null, null, null, 1, 10);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.WalletId)
                .WithErrorMessage("Wallet ID is required.");
        }

        [Fact]
        public void Should_HaveError_WhenPageIsZeroOrNegative()
        {
            // Arrange
            var query = new GetTransactionsQuery(Guid.NewGuid(), null, null, null, null, 0, 10);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Page)
                .WithErrorMessage("Page must be greater than 0.");
        }

        [Fact]
        public void Should_HaveError_WhenSizeIsOutOfRange()
        {
            // Arrange
            var query = new GetTransactionsQuery(Guid.NewGuid(), null, null, null, null, 1, 101);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Size)
                .WithErrorMessage("Size must be between 1 and 100.");
        }

        [Fact]
        public void Should_PassValidation_WhenAllFieldsValid()
        {
            // Arrange
            var query = new GetTransactionsQuery(Guid.NewGuid(), null, null, null, null, 1, 20);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
