using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Services;
using Wallet.Domain.Enums;

namespace Wallet.Application.Transactions.Refunds.Commands.RefundTransaction
{
    public class RefundTransactionCommandHandler : IRequestHandler<RefundTransactionCommand, Unit>
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUserValidator _userValidator;
        public RefundTransactionCommandHandler(ITransactionRepository transactionRepository, IWalletRepository walletRepository , IUserValidator userValidator)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
            _userValidator = userValidator;
        }

        public async Task<Unit> Handle(RefundTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);

            if (transaction is null)
                throw new KeyNotFoundException($"Transaction with ID {request.TransactionId} not found.");

            if (transaction.Status != TransactionStatus.Completed)
                throw new InvalidOperationException("Only completed transactions can be refunded.");

            var daysSinceTransaction = (DateTime.UtcNow - transaction.CreatedAt).TotalDays;
            //if transacton date is more than 7 days ago  , refund is not allowed
            if (daysSinceTransaction > 7)
                throw new InvalidOperationException("Refund period has expired. Refunds are only allowed within 7 days of the transaction.");

            transaction.MarkReversed("Refund initiated by user");

            var wallet = await _walletRepository.GetByIdAsync(transaction.WalletId, cancellationToken);
            if (wallet == null)
                throw new InvalidOperationException("Associated wallet not found.");

            await _userValidator.EnsureUserIsActiveAsync(wallet.UserId, cancellationToken);


            wallet.TopUp(transaction.Amount.Amount);

            await _walletRepository.UpdateAsync(wallet, cancellationToken);
            await _transactionRepository.UpdateAsync(transaction, cancellationToken);

            return Unit.Value;
        }
    }
}
