using MediatR;
using System;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.TopUp.DTOs;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;
using Wallet.Domain.Entities;
namespace Wallet.Application.Transactions.TopUp.Commands.TopUpWallet
{
    public class TopUpWalletCommandHandler : IRequestHandler<TopUpWalletCommand, TransactionDto>
    {

        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;

        public TopUpWalletCommandHandler(IWalletRepository walletRepository, ITransactionRepository transactionRepository , IUserRepository userRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public async Task<TransactionDto> Handle(TopUpWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId, cancellationToken);
            if (wallet is null)
                throw new KeyNotFoundException($"Wallet with ID {request.WalletId} not found.");

            if (wallet.Status != WalletStatus.Active)
                throw new InvalidOperationException("Only active wallets can receive top-ups.");

            var user = await _userRepository.GetByIdAsync(wallet.UserId, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {wallet.UserId} not found.");

            if (user.UserStatus != UserStatus.Active)
                throw new InvalidOperationException("Only active users can perform this operation.");


            var currency = Currency.FromCode(wallet.Balance.Currency.Code);
            var money = Money.Create(request.Amount, currency);

            var transaction = new Transaction(
                walletId: wallet.Id,
                amount: money,
                type: TransactionType.TopUp,
                description: request.Description

            );
            
            wallet.TopUp(request.Amount);

            transaction.MarkCompleted();
            await _transactionRepository.AddAsync(transaction, cancellationToken);
            await _walletRepository.UpdateAsync(wallet, cancellationToken);

            return new TransactionDto
            {
                Id = transaction.Id,
                WalletId = wallet.Id,
                Amount = request.Amount,
                CurrencyCode = currency.Code,
                Type = transaction.Type.ToString(),
                Status = transaction.Status.ToString(),
                Description = transaction.Description,
                CreatedAt = transaction.CreatedAt
            };
        }
    }
}
