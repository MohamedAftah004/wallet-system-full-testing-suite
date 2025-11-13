using FluentValidation.TestHelper;
using System;
using Wallet.Application.Transactions.Payments.Commands.MakePayment;
using Xunit;

namespace Wallet.Tests.Application.Transactions.Payments.Commands.MakePayment
{
    public class MakePaymentCommandValidatorTests
    {
        private readonly MakePaymentCommandValidator _validator;

        public MakePaymentCommandValidatorTests()
        {
            _validator = new MakePaymentCommandValidator();
        }

        [Fact]
        public void Should_HaveError_WhenWalletIdIsEmpty()
        {
            var command = new MakePaymentCommand(Guid.Empty, 100, "Test Payment");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.WalletId)
                .WithErrorMessage("Wallet ID is required.");
        }

        [Fact]
        public void Should_HaveError_WhenAmountIsZeroOrNegative()
        {
            var command = new MakePaymentCommand(Guid.NewGuid(), 0, "Invalid amount");

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Amount)
                .WithErrorMessage("Amount must be greater than zero.");
        }

        [Fact]
        public void Should_HaveError_WhenDescriptionTooLong()
        {
            var longDescription = new string('a', 201);
            var command = new MakePaymentCommand(Guid.NewGuid(), 100, longDescription);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Description must not exceed 200 characters.");
        }

        [Fact]
        public void Should_PassValidation_WhenAllFieldsValid()
        {
            var command = new MakePaymentCommand(Guid.NewGuid(), 150, "Valid payment");

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
