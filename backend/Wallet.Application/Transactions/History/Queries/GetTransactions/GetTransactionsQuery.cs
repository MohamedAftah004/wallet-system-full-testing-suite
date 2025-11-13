using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Models;
using Wallet.Application.Transactions.TopUp.DTOs;

namespace Wallet.Application.Transactions.History.Queries.GetTransactions
{
    public record GetTransactionsQuery(
        Guid WalletId,
        string? Type = null,
        string? Status = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        int Page = 1,
        int Size = 20
        ) : IRequest<PagedResult<TransactionDto>>;


}
