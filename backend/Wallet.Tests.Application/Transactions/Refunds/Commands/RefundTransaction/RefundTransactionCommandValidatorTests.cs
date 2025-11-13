using System;
using FluentValidation.TestHelper;
using Wallet.Application.Transactions.Refunds.Commands.RefundTransaction;
using Xunit;

namespace Wallet.Tests.Application.Transactions.Refunds.Commands
{
    public class RefundTransactionCommandValidatorTests
    {
        private readonly RefundTransactionCommandValidator _validator;

        public RefundTransactionCommandValidatorTests()
        {
            _validator = new RefundTransactionCommandValidator();
        }

        [Fact]
        public void Should_HaveError_WhenTransactionIdIsEmpty()
        {
            var command = new RefundTransactionCommand(Guid.Empty);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.TransactionId)
                .WithErrorMessage("Transaction ID is required.");
        }

        [Fact]
        public void Should_PassValidation_WhenTransactionIdIsValid()
        {
            var command = new RefundTransactionCommand(Guid.NewGuid());

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
