using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Models;
using Wallet.Application.Transactions.History.Queries.GetTransactions;
using Wallet.Application.Transactions.TopUp.DTOs;
using Wallet.Domain.Enums;


namespace Wallet.Application.Transactions.History.Queries.GetTransactions
{
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery , PagedResult<TransactionDto>>
    {

        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionsQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<PagedResult<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {

            var query = _transactionRepository.Query().Where(t=>t.WalletId == request.WalletId);

            //filter by type
            if(!string.IsNullOrEmpty(request.Type) && 
                Enum.TryParse<TransactionType>(request.Type , true , out var type))
                query = query.Where(t => t.Type == type);

            //filter by status
            if(!string.IsNullOrEmpty(request.Status) && 
                Enum.TryParse<TransactionStatus>(request.Status , true , out var status))
                query = query.Where(t => t.Status == status);

            //filter by date range
            if (request.ToDate.HasValue)
                query = query.Where(t => t.CreatedAt <= request.ToDate.Value);


            if (request.ToDate.HasValue)
                query = query.Where(t => t.CreatedAt <= request.ToDate.Value);

            
            var totalCount = await query.CountAsync(cancellationToken);


            var items = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((request.Page - 1) * request.Size)
                .Take(request.Size)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    WalletId = t.WalletId,
                    Amount = t.Amount.Amount,
                    CurrencyCode = t.Amount.Currency.Code,
                    Type = t.Type.ToString(),
                    Status = t.Status.ToString(),
                    Description = t.Description,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<TransactionDto>(items, totalCount, request.Page, request.Size);
        }
    }
}
