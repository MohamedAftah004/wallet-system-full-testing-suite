using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.Refunds.DTOs;
using Wallet.Domain.Enums;

namespace Wallet.Application.Transactions.Refunds.Queries.GetRefundsByWalletId
{
    public class GetRefundsByWalletIdQueryHandler : IRequestHandler<GetRefundsByWalletIdQuery, IEnumerable<RefundDto>>
    {

        private readonly ITransactionRepository _transactionRepository;

        public GetRefundsByWalletIdQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<RefundDto>> Handle(GetRefundsByWalletIdQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _transactionRepository.GetByWalletIdAsync(request.WalletId, cancellationToken);

            var refunds = transactions.Where(t => t.Status == TransactionStatus.Reversed)
                .Select(t => new RefundDto
                {
                    Id = t.Id,
                    WalletId = t.WalletId,
                    Amount = t.Amount.Amount,
                    CurrencyCode = t.Amount.Currency.Code,
                    Status = t.Status.ToString(),
                    Description = t.Description,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                }).ToList();

            if (!refunds.Any())
                throw new KeyNotFoundException($"No refunds found for wallet ID {request.WalletId}");

            return refunds;

        }
    }
}
