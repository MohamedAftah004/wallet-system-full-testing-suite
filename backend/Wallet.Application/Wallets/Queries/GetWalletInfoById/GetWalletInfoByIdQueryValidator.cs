using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Wallets.DTOs;

namespace Wallet.Application.Wallets.Queries.GetWalletInfoById
{
    public class GetWalletInfoByIdQueryValidator : AbstractValidator<GetWalletInfoByIdQuery>
    {
        public GetWalletInfoByIdQueryValidator()
        {
            RuleFor(x => x.WalletId).NotEmpty().WithMessage("WalletId is required");

            RuleFor(x => x.WalletId).Must(id => id != Guid.Empty).WithMessage("WalletId must be a valid GUID.");

        }
    }
}
