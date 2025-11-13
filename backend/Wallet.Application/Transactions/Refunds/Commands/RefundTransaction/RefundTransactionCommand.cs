using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Transactions.Refunds.Commands.RefundTransaction
{
    public record RefundTransactionCommand(Guid TransactionId) : IRequest<Unit>;
}
