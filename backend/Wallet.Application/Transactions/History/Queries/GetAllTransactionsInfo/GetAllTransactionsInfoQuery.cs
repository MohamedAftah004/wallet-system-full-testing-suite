using MediatR;
using Wallet.Application.Common.Models;
using Wallet.Application.Transactions.TopUp.DTOs;

namespace Wallet.Application.Transactions.History.Queries.GetAllTransactionsInfo
{
    public record GetAllTransactionsInfoQuery(
        string? Type = null,
        string? Status = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        int Page = 1,
        int Size = 20
    ) : IRequest<PagedResult<TransactionDto>>;
}
