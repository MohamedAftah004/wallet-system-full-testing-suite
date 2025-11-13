using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Transactions.TopUp.DTOs;

namespace Wallet.Application.Transactions.TopUp.Commands.TopUpWallet
{
    public record TopUpWalletCommand(Guid WalletId, decimal Amount, string Description) : IRequest<TransactionDto>;
}
