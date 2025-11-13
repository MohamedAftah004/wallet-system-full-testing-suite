using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Transactions.Payments.Queries.GetWalletBalance
{
    public class GetWalletBalanceQueryValidator : AbstractValidator<GetWalletBalanceQuery>
    {

        public GetWalletBalanceQueryValidator()
        {
            RuleFor(x=>x.WalletId).NotEmpty().WithMessage("WalletId is required.")
                .Must(id => id != Guid.Empty).WithMessage("WalletId must be a valid GUID.");
        }
    }
}
