using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.Payments.DTOs;

namespace Wallet.Application.Transactions.Payments.Queries.GetWalletBalance
{
    public class GetWalletBalanceQueryHandler : IRequestHandler<GetWalletBalanceQuery, WalletBalanceDto>
    {
        private readonly IWalletRepository _walletRepository;

        public GetWalletBalanceQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletBalanceDto> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId, cancellationToken);

            if (wallet is null)
                throw new KeyNotFoundException($"Wallet with ID {request.WalletId} not found.");

            return new WalletBalanceDto
            {
                WalletId = wallet.Id,
                Amount = wallet.Balance.Amount,
                CurrencyCode = wallet.Balance.Currency.Code,
                CurrencySymbol = wallet.Balance.Currency.Symbol ?? string.Empty,
                LastUpdated = wallet.UpdatedAt ?? wallet.CreatedAt
            };

        }
    }
}
