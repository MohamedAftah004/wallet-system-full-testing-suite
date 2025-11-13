using FluentValidation.TestHelper;
using System;
using Wallet.Application.Transactions.TopUp.Queries.GetTransactionById;
using Xunit;

namespace Wallet.Tests.Application.Transactions.TopUp.Queries.GetTransactionById
{
    public class GetTransactionByIdQueryValidatorTests
    {
        private readonly GetTransactionByIdQueryValidator _validator;

        public GetTransactionByIdQueryValidatorTests()
        {
            _validator = new GetTransactionByIdQueryValidator();
        }

        [Fact]
        public void Should_HaveError_WhenTransactionIdIsEmpty()
        {
            var query = new GetTransactionByIdQuery(Guid.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.TransactionId)
                .WithErrorMessage("TransactionId is required to retrieve Transaction information.");
        }

        [Fact]
        public void Should_PassValidation_WhenTransactionIdIsValid()
        {
            var query = new GetTransactionByIdQuery(Guid.NewGuid());

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
