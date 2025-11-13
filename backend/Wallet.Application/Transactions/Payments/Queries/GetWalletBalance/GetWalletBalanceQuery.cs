using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Transactions.Payments.DTOs;

namespace Wallet.Application.Transactions.Payments.Queries.GetWalletBalance
{
    public record GetWalletBalanceQuery(Guid WalletId) : IRequest<WalletBalanceDto>;
}
