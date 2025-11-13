using System;
using FluentValidation.TestHelper;
using Wallet.Application.Transactions.Payments.Queries.GetWalletBalance;
using Xunit;

namespace Wallet.Tests.Application.Transactions.Payments.Queries.GetWalletBalance
{
    public class GetWalletBalanceQueryValidatorTests
    {
        private readonly GetWalletBalanceQueryValidator _validator;

        public GetWalletBalanceQueryValidatorTests()
        {
            _validator = new GetWalletBalanceQueryValidator();
        }

        [Fact]
        public void Should_HaveError_WhenWalletIdIsEmpty()
        {
            var query = new GetWalletBalanceQuery(Guid.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.WalletId)
                .WithErrorMessage("WalletId is required.");
        }

        [Fact]
        public void Should_HaveError_WhenWalletIdIsInvalidGuid()
        {
            var query = new GetWalletBalanceQuery(Guid.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.WalletId)
                .WithErrorMessage("WalletId must be a valid GUID.");
        }

        [Fact]
        public void Should_PassValidation_WhenWalletIdIsValid()
        {
            var query = new GetWalletBalanceQuery(Guid.NewGuid());

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
