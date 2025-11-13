using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.TopUp.DTOs;

namespace Wallet.Application.Transactions.History.Queries.GetRecentTransactions
{
    public class GetRecentTransactionsQueryHandler
        : IRequestHandler<GetRecentTransactionsQuery, IReadOnlyList<TransactionDto>>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetRecentTransactionsQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IReadOnlyList<TransactionDto>> Handle(GetRecentTransactionsQuery request, CancellationToken cancellationToken)
        {
            var query = _transactionRepository.Query();

            if (request.WalletId.HasValue)
                query = query.Where(t => t.WalletId == request.WalletId.Value);

            var recentTransactions = await query
                .OrderByDescending(t => t.CreatedAt)
                .Take(request.Count)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    WalletId = t.WalletId,
                    Amount = t.Amount.Amount,
                    CurrencyCode = t.Amount.Currency.Code,
                    Type = t.Type.ToString(),
                    Status = t.Status.ToString(),
                    Description = t.Description,
                    CreatedAt = t.CreatedAt,
                    UserFullName = t.Wallet.User.FullName

                })
                .ToListAsync(cancellationToken);

            return recentTransactions;
        }
    }
}
