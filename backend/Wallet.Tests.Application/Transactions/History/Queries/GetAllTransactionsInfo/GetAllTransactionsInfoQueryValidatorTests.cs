using System;
using FluentValidation.TestHelper;
using Wallet.Application.Transactions.History.Queries.GetAllTransactionsInfo;
using Xunit;

namespace Wallet.Tests.Application.Transactions.History.Queries.GetAllTransactionsInfo
{
    public class GetAllTransactionsInfoQueryValidatorTests
    {
        private readonly GetAllTransactionsInfoQueryValidator _validator;

        public GetAllTransactionsInfoQueryValidatorTests()
        {
            _validator = new GetAllTransactionsInfoQueryValidator();
        }

        [Fact]
        public void Should_HaveError_WhenPageIsZero()
        {
            // Arrange
            var query = new GetAllTransactionsInfoQuery { Page = 0, Size = 10 };

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
            var query = new GetAllTransactionsInfoQuery { Page = 1, Size = 150 };

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Size)
                .WithErrorMessage("Page size must be between 1 and 100.");
        }

        [Fact]
        public void Should_HaveError_WhenToDateIsBeforeFromDate()
        {
            // Arrange
            var query = new GetAllTransactionsInfoQuery
            {
                FromDate = DateTime.UtcNow,
                ToDate = DateTime.UtcNow.AddDays(-1),
                Page = 1,
                Size = 10
            };

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ToDate)
                .WithErrorMessage("ToDate must be greater than or equal to FromDate.");
        }

        [Fact]
        public void Should_PassValidation_WhenQueryIsValid()
        {
            // Arrange
            var query = new GetAllTransactionsInfoQuery
            {
                Page = 1,
                Size = 20,
                FromDate = DateTime.UtcNow.AddDays(-2),
                ToDate = DateTime.UtcNow
            };

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
