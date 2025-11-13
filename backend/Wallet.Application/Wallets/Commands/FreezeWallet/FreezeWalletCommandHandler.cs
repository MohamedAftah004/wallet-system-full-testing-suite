using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Enums;

namespace Wallet.Application.Wallets.Commands.FreezeWallet
{
    public class FreezeWalletCommandHandler : IRequestHandler<FreezeWalletCommand, Unit>
    {
        private readonly IWalletRepository _walletRepository;

        public FreezeWalletCommandHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Unit> Handle(FreezeWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId, cancellationToken);

            if (wallet is null)
                throw new KeyNotFoundException($"Wallet with ID {request.WalletId} not found.");

            if (wallet.Status == WalletStatus.Frozen)
                throw new InvalidOperationException("Wallet is already frozen.");

            if (wallet.Status == WalletStatus.Disabled)
                throw new InvalidOperationException("Cannot freeze a disabled wallet.");

            wallet.Freeze();

            await _walletRepository.UpdateAsync(wallet, cancellationToken);

            return Unit.Value;
        }
    }
}
