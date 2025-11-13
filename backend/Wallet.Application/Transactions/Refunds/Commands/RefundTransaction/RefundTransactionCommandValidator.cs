using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wallet.Application.Transactions.Refunds.Commands.RefundTransaction
{
    public class RefundTransactionCommandValidator : AbstractValidator<RefundTransactionCommand>
    {

        public RefundTransactionCommandValidator()
        {
            RuleFor(x => x.TransactionId)
          .NotEmpty().WithMessage("Transaction ID is required.");
        }

    }
}
