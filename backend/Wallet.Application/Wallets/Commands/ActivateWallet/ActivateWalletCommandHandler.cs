using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Enums;

namespace Wallet.Application.Wallets.Commands.ActivateWallet
{
    public class ActivateWalletCommandHandler : IRequestHandler<ActivateWalletCommand, Unit>
    {
        private readonly IWalletRepository _walletRepository;

        public ActivateWalletCommandHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Unit> Handle(ActivateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId , cancellationToken);    
            if(wallet is null)
                throw new KeyNotFoundException($"Wallet with ID {request.WalletId} not found.");

            if (wallet.Status == WalletStatus.Active)
                throw new InvalidOperationException("Wallet is already active.");

            if (wallet.Status == WalletStatus.Disabled)
                throw new InvalidOperationException("Wallet is disabled and cannot be reactivated.");

            if (wallet.Status == WalletStatus.Frozen)
                throw new InvalidOperationException("Wallet is frozen and must be unfrozen before activation.");

            if (wallet.Status != WalletStatus.PendingActivation)
                throw new InvalidOperationException($"Wallet cannot be activated from status '{wallet.Status}'.");

            wallet.Activate();
            await _walletRepository.UpdateAsync(wallet, cancellationToken);

            return Unit.Value;
        }
    }
}
