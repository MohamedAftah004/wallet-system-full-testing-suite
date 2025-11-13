using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Transactions.Refunds.Queries.GetRefundsByWalletId
{
    public class GetRefundsByWalletIdQueryValidator : AbstractValidator<GetRefundsByWalletIdQuery>
    {

        public GetRefundsByWalletIdQueryValidator()
        {
            RuleFor(x => x.WalletId)
           .NotEmpty().WithMessage("Wallet ID is required.");
        }

    }
}
