using MediatR;
using Wallet.Application.Transactions.TopUp.DTOs;
using System.Collections.Generic;

namespace Wallet.Application.Transactions.History.Queries.GetRecentTransactions
{
    public record GetRecentTransactionsQuery(
        Guid? WalletId = null,
        int Count = 5
    ) : IRequest<IReadOnlyList<TransactionDto>>;
}
