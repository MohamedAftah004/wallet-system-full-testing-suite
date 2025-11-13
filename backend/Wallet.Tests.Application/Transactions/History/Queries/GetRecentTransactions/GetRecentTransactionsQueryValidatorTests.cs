using FluentValidation.TestHelper;
using Wallet.Application.Transactions.History.Queries.GetRecentTransactions;
using Xunit;

namespace Wallet.Tests.Application.Transactions.History.Queries.GetRecentTransactions
{
    public class GetRecentTransactionsQueryValidatorTests
    {
        private readonly GetRecentTransactionsQueryValidator _validator;

        public GetRecentTransactionsQueryValidatorTests()
        {
            _validator = new GetRecentTransactionsQueryValidator();
        }

        [Fact]
        public void Should_HaveError_WhenCountIsLessThanOne()
        {
            var query = new GetRecentTransactionsQuery { Count = 0 };

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.Count)
                .WithErrorMessage("Count must be between 1 and 100.");
        }

        [Fact]
        public void Should_HaveError_WhenCountIsMoreThan100()
        {
            var query = new GetRecentTransactionsQuery { Count = 101 };

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.Count)
                .WithErrorMessage("Count must be between 1 and 100.");
        }

        [Fact]
        public void Should_PassValidation_WhenCountIsValid()
        {
            var query = new GetRecentTransactionsQuery { Count = 10 };

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
