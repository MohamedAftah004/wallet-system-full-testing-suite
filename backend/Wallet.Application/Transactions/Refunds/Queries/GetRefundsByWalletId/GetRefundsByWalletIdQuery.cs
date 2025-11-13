using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Transactions.Refunds.DTOs;

namespace Wallet.Application.Transactions.Refunds.Queries.GetRefundsByWalletId
{
    public record GetRefundsByWalletIdQuery(Guid WalletId) : IRequest<IEnumerable<RefundDto>>;
}
