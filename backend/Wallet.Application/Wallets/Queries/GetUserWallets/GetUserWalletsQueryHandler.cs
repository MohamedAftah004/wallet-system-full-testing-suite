using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Wallets.DTOs;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;

namespace Wallet.Application.Wallets.Queries.GetUserWallets
{
    public class GetUserWalletsQueryHandler : IRequestHandler<GetUserWalletsQuery, IEnumerable<WalletDto>>
    {

        private readonly IWalletRepository _walletRepository;

        public GetUserWalletsQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }
        public async Task<IEnumerable<WalletDto>> Handle(GetUserWalletsQuery request, CancellationToken cancellationToken)
        {
            var wallets = await _walletRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (wallets == null || !wallets.Any())
                throw new KeyNotFoundException($"No wallets found for user ID {request.UserId}");

            return wallets.Select(wallet => new WalletDto
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
                        WalletStatus.Disabled => "Wallet has been disabled permanently",
                        _ => "Unknown status"
                    }
                },
                CreatedAt = wallet.CreatedAt,
                UpdatedAt = wallet.UpdatedAt
            }).ToList();
        }
    }
    
    }
