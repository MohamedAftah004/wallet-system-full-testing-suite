using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Services;
using Wallet.Application.Transactions.Payments.DTOs;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.ValueObjects;

namespace Wallet.Application.Transactions.Payments.Commands.MakePayment
{
    public class MakePaymentCommandHandler : IRequestHandler<MakePaymentCommand, PaymentResponseDto>
    {

        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserValidator _userValidator;
        public MakePaymentCommandHandler(IWalletRepository walletRepository, ITransactionRepository transactionRepository , IUserValidator userValidator)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
            _userValidator = userValidator;
        }

        public async Task<PaymentResponseDto> Handle(MakePaymentCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId, cancellationToken);
            if (wallet is null)
                throw new KeyNotFoundException($"Wallet with ID {request.WalletId} not found.");

            if (wallet.Status != WalletStatus.Active)
                throw new InvalidOperationException("Only active wallets can make payments.");

            if (wallet.Balance.Amount < request.Amount)
                throw new InvalidOperationException("Insufficient balance.");
 
            await _userValidator.EnsureUserIsActiveAsync(wallet.UserId, cancellationToken);

            var money = Money.Create(request.Amount, wallet.Balance.Currency);

            wallet.Deduct(request.Amount);

            var transaction = new Transaction(
              walletId: wallet.Id,
              amount: money,
              type: TransactionType.Payment,
              description: request.Description
          );

            transaction.MarkCompleted();

            await _transactionRepository.AddAsync(transaction, cancellationToken);
            await _walletRepository.UpdateAsync(wallet, cancellationToken);

            return new PaymentResponseDto
            {
                TransactionId = transaction.Id,
                WalletId = wallet.Id,
                Amount = transaction.Amount.Amount,
                CurrencyCode = transaction.Amount.Currency.Code,
                Status = transaction.Status.ToString(),
                Description = transaction.Description,
                CreatedAt = transaction.CreatedAt
            };

        }
    }
}
