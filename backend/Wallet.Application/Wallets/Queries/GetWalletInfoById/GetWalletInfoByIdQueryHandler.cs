using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Wallets.DTOs;
using Wallet.Domain.Enums;

namespace Wallet.Application.Wallets.Queries.GetWalletInfoById
{
    public class GetWalletInfoByIdQueryHandler : IRequestHandler<GetWalletInfoByIdQuery, WalletDto>
    {

        public readonly IWalletRepository _walletReposiotry;

        public GetWalletInfoByIdQueryHandler(IWalletRepository walletReposiotry)
        {
            _walletReposiotry = walletReposiotry;
        }

        public async Task<WalletDto> Handle(GetWalletInfoByIdQuery request, CancellationToken cancellationToken)
        {

            var wallet = await _walletReposiotry.GetByIdAsync(request.WalletId);

            if(wallet is null)
                throw new KeyNotFoundException($"Wallet with ID {request.WalletId} not found.");

            return new WalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = new BalanceDto
                {
                    Amount = wallet.Balance.Amount,
                    CurrencyCode = wallet.Balance.Currency.Code,
                    CurrencySymbol = wallet.Balance.Currency.Symbol
                },

                Status = new StatusDto
                {
                    Code = wallet.Status.ToString(),
                    Description = wallet.Status switch
                    {
                        WalletStatus.PendingActivation => "Wallet is created but not yet activated",
                        WalletStatus.Active => "Wallet is active and ready for transactions",
                        WalletStatus.Frozen => "Wallet is temporarily frozen",
                        WalletStatus.Closed => "Wallet has been closed",
                        _ => "Unknown status"
                    }
                },

                CreatedAt = wallet.CreatedAt,
                UpdatedAt = wallet.UpdatedAt
            };


        }
    }
}
