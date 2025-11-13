using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Wallets.DTOs;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using WalletEntity = Wallet.Domain.Entities.Wallet;

namespace Wallet.Application.Wallets.Commands.CreateWallet
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, CreateWalletResponseDto >
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;

        public CreateWalletCommandHandler(IUserRepository userRepository, IWalletRepository walletRepository)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
        }

        public async Task<CreateWalletResponseDto> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null)
                throw new EntityNotFoundException(EntityType.User, request.UserId.ToString());

            var currency = Currency.FromCode(request.CurrencyCode);

            var wallet = new WalletEntity(
                request.UserId,
                currency,
                WalletStatus.PendingActivation
            );

            await _walletRepository.AddAsync(wallet, cancellationToken);

            return new CreateWalletResponseDto
            {
                WalletId = wallet.Id,
                UserId = wallet.UserId,
                Currency = wallet.Balance.Currency.Code,
                Balance = wallet.Balance.Amount,
                Status = wallet.Status.ToString(),
                CreatedAt = wallet.CreatedAt
            };
        }
    }
}
