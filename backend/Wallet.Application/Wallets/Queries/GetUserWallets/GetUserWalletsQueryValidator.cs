using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Wallets.Queries.GetUserWallets
{
    public class GetUserWalletsQueryValidator : AbstractValidator<GetUserWalletsQuery>
    {
        public GetUserWalletsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required.");
        }
    }
}
