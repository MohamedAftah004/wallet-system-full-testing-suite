using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Entities;
namespace Wallet.Application.Common.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default);
        Task<Transaction?> GetByIdAsync(Guid transactionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId, CancellationToken cancellationToken = default);
        Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default);

        IQueryable<Transaction> Query();
    }
}
