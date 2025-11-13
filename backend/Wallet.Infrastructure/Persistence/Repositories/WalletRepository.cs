using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Common.Interfaces;
using Wallet.Infrastructure.Persistence;
using WalletEntity = Wallet.Domain.Entities.Wallet;

namespace Wallet.Infrastructure.Persistence.Repositories
{
    public class WalletRepository : IWalletRepository
    {

        private readonly AppDbContext _dbContext;
        public WalletRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// create new wallet.
        /// </summary>
        public async Task AddAsync(WalletEntity wallet, CancellationToken cancellationToken = default)
        {
            await _dbContext.Wallets.AddAsync(wallet);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Get all wallets that belong to a specific user.
        /// </summary>
        public async Task<IEnumerable<WalletEntity>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Wallets
                .Where(w => w.UserId == userId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }


        /// <summary>
        /// Get wallet by walletId.
        /// </summary>
        public async Task<WalletEntity?> GetByIdAsync(Guid walletId, CancellationToken cancellationToken = default)
        {
            if (walletId == Guid.Empty)
                return null; 

            return await _dbContext.Wallets
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == walletId, cancellationToken);
        }

        /// <summary>
        /// update wallet info.
        /// </summary>
        public async Task UpdateAsync(WalletEntity wallet, CancellationToken cancellationToken = default)
        {
            _dbContext.Wallets.Update(wallet);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

      

    }
}
