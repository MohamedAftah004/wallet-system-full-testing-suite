using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;

namespace Wallet.Application.Wallets.Commands.CloseWallet
{
    public class CloseWalletCommandHandler : IRequestHandler<CloseWalletCommand, Unit>
    {

        private readonly IWalletRepository _walletRepository;

        public CloseWalletCommandHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Unit> Handle(CloseWalletCommand request, CancellationToken cancellationToken)
        {

            var wallet = await _walletRepository.GetByIdAsync(request.WalletId, cancellationToken);


            if (wallet is null)
                throw new KeyNotFoundException($"Wallet with ID {request.WalletId} not found.");

            if (wallet.Status == WalletStatus.Disabled)
                throw new InvalidOperationException("Wallet is already closed.");

            wallet.Disable();

            await _walletRepository.UpdateAsync(wallet, cancellationToken);

            return Unit.Value;
        }
    }
}
