using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Transactions.TopUp.DTOs;
using Wallet.Application.Users.DTOs;
using Wallet.Domain.Enums;

namespace Wallet.Application.Users.Queries.GetUserDashboard
{
    public class GetUserDashboardQueryHandler
        : IRequestHandler<GetUserDashboardQuery, UserDashboardDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
         
        public GetUserDashboardQueryHandler(
            IUserRepository userRepository,
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<UserDashboardDto> Handle(GetUserDashboardQuery request, CancellationToken cancellationToken)
        {
             var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                throw new Exception($"User with ID {request.UserId} not found.");

             var wallets = await _walletRepository.GetByUserIdAsync(user.Id, cancellationToken);
            if (wallets == null || !wallets.Any())
                throw new Exception($"No wallets found for user {user.FullName}.");

             var walletBalance = wallets.Sum(w => w.Balance.Amount);

             var walletIds = wallets.Select(w => w.Id).ToList();

             var transactions = _transactionRepository.Query()
                .Where(t => walletIds.Contains(t.WalletId));

            var totalTransactions = await transactions.CountAsync(cancellationToken);
            var completedTransactions = await transactions.CountAsync(
                t => t.Status == TransactionStatus.Completed, cancellationToken);
            var pendingTransactions = await transactions.CountAsync(
                t => t.Status == TransactionStatus.Pending, cancellationToken);

             var recentTransactions = await transactions
                .OrderByDescending(t => t.CreatedAt)
                .Take(5)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    WalletId = t.WalletId,
                    Amount = t.Amount.Amount,
                    CurrencyCode = t.Amount.Currency.Code,
                    Type = t.Type.ToString(),
                    Status = t.Status.ToString(),
                    Description = t.Description,
                    CreatedAt = t.CreatedAt,
                    UserFullName = user.FullName
                })
                .ToListAsync(cancellationToken);

             return new UserDashboardDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                WalletBalance = walletBalance,
                TotalTransactions = totalTransactions,
                CompletedTransactions = completedTransactions,
                PendingTransactions = pendingTransactions,
                RecentTransactions = recentTransactions
            };
        }
    }
}
