using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.TopUp.DTOs;

namespace Wallet.Application.Transactions.TopUp.Queries.GetTransactionById
{
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionResponseDto>
    {

        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionResponseDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);

            if (transaction == null)
                throw new KeyNotFoundException($"Transaction with ID {request.TransactionId} not found.");

            return new TransactionResponseDto
            {
                Id = transaction.Id,
                WalletId = transaction.WalletId,
                Amount = transaction.Amount.Amount,
                CurrencyCode = transaction.Amount.Currency.Code,
                CurrencySymbol = transaction.Amount.Currency.Symbol,
                Type = transaction.Type.ToString(),
                Status = TransactionStatus.Committed.ToString(),
                ReferenceId = transaction.ReferenceId,
                Description = transaction.Description,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };
        }
    }
}
