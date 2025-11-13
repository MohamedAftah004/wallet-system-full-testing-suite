using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Transaction = Wallet.Domain.Entities.Transaction;

namespace Wallet.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _dbContext;

        public TransactionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Add a new transaction.
        /// </summary>
        public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            await _dbContext.Transactions.AddAsync(transaction, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }



        /// <summary>
        /// Get transaction by ID.
        /// </summary>
        public async Task<Transaction?> GetByIdAsync(Guid transactionId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == transactionId, cancellationToken);
        }

        /// <summary>
        /// Get all transactions for a specific wallet.
        /// </summary>
        public async Task<IEnumerable<Transaction>> GetByWalletIdAsync(Guid walletId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Transactions
                .Where(t => t.WalletId == walletId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Update transaction status or description.
        /// </summary>
        public async Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            _dbContext.Transactions.Update(transaction);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(System.Transactions.Transaction transaction, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }



        public IQueryable<Transaction> Query()
        {
            return _dbContext.Transactions.AsQueryable();
        }
    }
}
