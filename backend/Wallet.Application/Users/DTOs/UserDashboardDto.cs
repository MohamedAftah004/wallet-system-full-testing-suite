using System;
using System.Collections.Generic;
using Wallet.Application.Transactions.TopUp.DTOs;

namespace Wallet.Application.Users.DTOs
{
    public class UserDashboardDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public decimal WalletBalance { get; set; }
        public int TotalTransactions { get; set; }
        public int CompletedTransactions { get; set; }
        public int PendingTransactions { get; set; }
        public IReadOnlyList<TransactionDto> RecentTransactions { get; set; } = new List<TransactionDto>();
    }
}
