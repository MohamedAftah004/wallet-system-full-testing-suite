using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Transactions.History.Queries.GetTransactions
{
    public class GetTransactionsQueryValidator : AbstractValidator<GetTransactionsQuery>
    {

        public GetTransactionsQueryValidator()
        {
            RuleFor(x => x.WalletId)
              .NotEmpty().WithMessage("Wallet ID is required.");

            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(x => x.Size)
                .InclusiveBetween(1, 100).WithMessage("Size must be between 1 and 100.");
        }

    }
}
