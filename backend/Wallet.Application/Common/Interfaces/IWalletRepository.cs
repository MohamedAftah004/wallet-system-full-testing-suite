using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wallet.Domain.Entities;
using WalletEntity = Wallet.Domain.Entities.Wallet;

namespace Wallet.Application.Common.Interfaces
{
    public interface IWalletRepository
    {

      
        Task AddAsync(WalletEntity wallet, CancellationToken cancellationToken = default);
        Task <IEnumerable<WalletEntity>> GetByUserIdAsync(Guid userId,  CancellationToken cancellationToken = default);
        Task<WalletEntity?> GetByIdAsync(Guid walletId, CancellationToken cancellationToken = default);
        Task UpdateAsync(WalletEntity wallet, CancellationToken cancellationToken = default);

    }
}
